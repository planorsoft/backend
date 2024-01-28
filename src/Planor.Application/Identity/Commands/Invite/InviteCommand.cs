using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Application.Identity.Commands.ConfirmEmail;
using Planor.Domain.Constants;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Identity.Commands.Invite;

public class InviteCommand : IRequest<bool>
{
    public List<string> Emails { get; set; } = null!;
}

public class InviteCommandHandler : IRequestHandler<InviteCommand, bool>
{
    private readonly UserManager<User> _userManager;
    private readonly INotificationService _notificationService;
    private readonly ICurrentUserService _currentUserService;

    public InviteCommandHandler(
        UserManager<User> userManager,
        INotificationService notificationService,
        ICurrentUserService currentUserService)
    {
        _userManager = userManager;
        _notificationService = notificationService;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(InviteCommand request, CancellationToken cancellationToken)
    {
        foreach (var email in request.Emails)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.Email == email && x.TenantId == _currentUserService.TenantId,
                    cancellationToken: cancellationToken);
            if (user is not null)
            {
                throw new BadRequestException($"email: {email} sisteme zaten kayıtlı, lütfen tekrar deneyin.");
            }
        }

        foreach (var email in request.Emails)
        {
            var password = "myStrongPassword123!";
            var user = new User
            {
                Name = "Lütfen isminizi güncelleyin",
                Email = email,
                UserName = $"{email}-{_currentUserService.TenantId}",
                TenantId = _currentUserService.TenantId,
                EmailConfirmed = true
            };

            await _userManager.AddPasswordAsync(user, password);

            await _userManager.CreateAsync(user);

            await _userManager.AddToRoleAsync(user, Domain.Constants.Roles.Employee);
            await _userManager.AddToRoleAsync(user, Domain.Constants.Roles.TagRead);
            await _userManager.AddToRoleAsync(user, Domain.Constants.Roles.MailTemplateRead);

            var data = new
            {
                Title = $"Planor uygulamasına {_currentUserService.Name} tarafından davet edildiniz.",
                Message =
                    "Aşağıdaki bilgileri kullanarak sisteme giriş yapabilirsin. Giriş yaptıktan sonra şifreni güncellemeyi unutma!",
                Email = email,
                Password = password,
            };


            _notificationService.Send(NotificationType.InviteUser, data, user);
        }

        return true;
    }
}