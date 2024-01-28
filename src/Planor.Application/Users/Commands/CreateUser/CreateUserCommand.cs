using System.Globalization;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Users.Commands.CreateUser;

public class CreateUserCommand : IRequest
{
    public string Email { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly ICurrentUserService _currentUserService;
    private readonly INotificationService _notificationService;
    private readonly CultureInfo _culture;


    public CreateUserCommandHandler(
        IApplicationDbContext context, 
        UserManager<User> userManager,
        ICurrentUserService currentUserService,
        INotificationService notificationService)
    {
        _context = context;
        _userManager = userManager;
        _currentUserService = currentUserService;
        _notificationService = notificationService;
        _culture = new CultureInfo("en-US");
    }

    public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var username = $"{request.Email}-{_currentUserService.TenantId}";

        var user = new User
        {
            Name = _culture.TextInfo.ToTitleCase(request.Name),
            UserName = username,
            Email = request.Email,
            TenantId = _currentUserService.TenantId,
            EmailConfirmed = true
        };

        await _userManager.AddPasswordAsync(user, request.Password);
        
        await _userManager.CreateAsync(user);
        
        await _userManager.AddToRolesAsync(user, Domain.Constants.Roles.EmployeeRoles);
        
        var data = new
        {
            Title = $"Planor uygulamasına {_currentUserService.Name} tarafından davet edildiniz.",
            Name = user.Name,
            Message = "Aşağıdaki bilgileri kullanarak sisteme giriş yapabilirsin. Giriş yaptıktan sonra şifreni güncelleyebilirsin.",
            Email = request.Email,
            Password = request.Password,
            Link = $"https://{_currentUserService.TenantId}.planorsoft.com/login"
        };
        
        _notificationService.Send(Domain.Constants.NotificationType.InviteUser, data, user);
    }
}