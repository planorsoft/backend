using System.Text;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Application.Customers.Commands.InviteContactToCustomer;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Customers.Commands.InviteContactToCustomer;

public class InviteContactToCustomerCommand : IRequest
{
    public int CustomerId { get; set; }
    public string Email { get; set; } = null!;
}

public class InviteContactToCustomerCommandHandler : IRequestHandler<InviteContactToCustomerCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly INotificationService _notificationService;
    private readonly ICurrentUserService _currentUserService;

    public InviteContactToCustomerCommandHandler(
        IApplicationDbContext context,
        UserManager<User> userManager,
        INotificationService notificationService,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _userManager = userManager;
        _notificationService = notificationService;
        _currentUserService = currentUserService;
    }

    public async Task Handle(InviteContactToCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == request.CustomerId, cancellationToken);
        if (customer is null) throw new NotFoundException(nameof(Customer), request.CustomerId);

        var user = await _userManager.Users.FirstOrDefaultAsync(x =>
                x.Email == request.Email && x.TenantId == _currentUserService.TenantId,
            cancellationToken: cancellationToken);
        if (user is null) throw new NotFoundException(nameof(User), request.Email);

        var password = GenerateRandomPassword();

        if (await _userManager.HasPasswordAsync(user))
        {
            await _userManager.RemovePasswordAsync(user);
        }

        await _userManager.AddPasswordAsync(user, password);

        var data = new
        {
            Title = $"Planor uygulamasına {_currentUserService.Name} tarafından davet edildiniz.",
            Name = user.Name,
            Message =
                "Aşağıdaki bilgileri kullanarak sisteme giriş yapabilirsin. Giriş yaptıktan sonra şifreni güncelleyebilirsin.",
            Email = request.Email,
            Password = password,
            Link = $"https://{_currentUserService.TenantId}.planorsoft.com/login"
        };

        _notificationService.Send(Domain.Constants.NotificationType.InviteUser, data, user);
    }

    private string GenerateRandomPassword()
    {
        var length = 15;
        const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ123456789";
        StringBuilder res = new StringBuilder();
        Random rnd = new Random();

        while (0 < length--)
        {
            if (length % 5 == 0 && length != 0)
            {
                res.Append("-");
            }
            else
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
        }

        return res.ToString();
    }
}