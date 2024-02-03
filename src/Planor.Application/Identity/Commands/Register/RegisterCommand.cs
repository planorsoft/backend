using System.Globalization;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Identity.Commands.Register;

public class RegisterCommand : IRequest<bool>
{
    public string Name { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string Tenant { get; set; } = null!;
}

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly INotificationService _notificationService;
    private readonly ITenantService _tenantService;
    private readonly IIdentityService _identityService;
    private readonly IDataSeedService _dataSeedService;
    private readonly CultureInfo _culture;

    public RegisterCommandHandler(
        IApplicationDbContext context,
        UserManager<User> userManager,
        INotificationService notificationService,
        ITenantService tenantService,
        IIdentityService identityService,
        IDataSeedService dataSeedService)
    {
        _context = context;
        _userManager = userManager;
        _notificationService = notificationService;
        _tenantService = tenantService;
        _identityService = identityService;
        _dataSeedService = dataSeedService;
        _culture = new CultureInfo("en-US");
    }

    public async Task<bool> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // Eğer Manager veya Customer değil ise 
        if (request.Role != Domain.Constants.Roles.Manager && request.Role != Domain.Constants.Roles.Customer)
        {
            throw new BadRequestException("Manager veya Customer olmayan kullanıcı yeni hesap oluşturamaz.");
        }

        // Eğer Customer var olmayan bir tenant için istek yaparsa
        if (request.Role == Domain.Constants.Roles.Customer && !_tenantService.AnyTenant(request.Tenant))
        {
            throw new BadRequestException("Böyle bir tenant bulunmuyor");
        }

        if (request.Role == Domain.Constants.Roles.Manager)
        {
            await _tenantService.CreateTenantAsync(request.Tenant);
        }

        var username = $"{request.Email}-{request.Tenant}";

        var user = new User
        {
            Name = _culture.TextInfo.ToTitleCase(request.Name),
            UserName = username,
            Email = request.Email,
            TenantId = request.Tenant
        };

        await _userManager.AddPasswordAsync(user, request.Password);

        await _userManager.CreateAsync(user);

        if (request.Role == Domain.Constants.Roles.Manager)
        {
            await _userManager.AddToRolesAsync(user, Domain.Constants.Roles.ManagerRoles);
            await _dataSeedService.SeedAsync(request.Tenant);
        }

        if (request.Role == Domain.Constants.Roles.Customer)
        {
            await _userManager.AddToRolesAsync(user, Domain.Constants.Roles.CustomerRoles);
            await CreateCustomerFromUser(user, cancellationToken);
        }

        var token = await _identityService.CreateMailConfirmationToken(user);

        var data = new
        {
            Title = "Mail adresini onayla, Planor ile plan yapmaya başla!",
            User = request.Name,
            Message = "Aşağıdaki kodu kullanarak mail adresini onaylayabilirsin.",
            Code = token,
        };

        _notificationService.Send(Domain.Constants.NotificationType.MailConfirmation, data, user);
        return true;
    }

    private async Task CreateCustomerFromUser(User user, CancellationToken cancellationToken)
    {
        var currency = await _context
            .Currencies
            .FirstOrDefaultAsync(x => x.IsDefault == true, cancellationToken);

        var customer = new Customer
        {
            Name = user.Name ?? user.UserName ?? "Müşteri",
            Contacts = new List<User>() { user },
            Currency = currency,
            TenantId = user.TenantId ?? "shared"
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync(cancellationToken);

        user.Customer = customer;
        user.CustomerId = customer.Id;
        await _userManager.UpdateAsync(user);
    }
}