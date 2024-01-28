using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Planor.Application.Common.Interfaces;
using Planor.Application.Identity.Commands.Invite;
using Planor.Domain.Constants;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Identity.Commands.Forgot;

public class ForgotCommand : IRequest<bool>
{
    public string Email { get; set; } = null!;
    public string Tenant { get; set; } = null!;
}

public class ForgotCommandHandler : IRequestHandler<ForgotCommand, bool>
{
    private readonly UserManager<User> _userManager;
    private readonly INotificationService _notificationService;
    private readonly IConfiguration _configuration;

    public ForgotCommandHandler(
        UserManager<User> userManager,
        INotificationService notificationService,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _notificationService = notificationService;
        _configuration = configuration;
    }

    public async Task<bool> Handle(ForgotCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(x => x.Email == request.Email && x.TenantId == request.Tenant,
                cancellationToken: cancellationToken);

        if (user is null) throw new NotFoundException(nameof(User), request.Email);

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var clientUrl = _configuration.GetSection("ClientUrl").Value ?? "localhost:3030";

        var data = new
        {
            Title = $"Şifreni sıfırlamak için aşağıdaki linki kullanabilirsin.",
            Message = $"Açılan sayfada yeni şifreni belirleyip şifreni güncelleyebilirsin.",
            User = request.Email,
            Link = $"http://{request.Tenant}.{clientUrl}/forgot-confirm-password?email={request.Email}&token={token}",
        };

        _notificationService.Send(Domain.Constants.NotificationType.ForgotMail, data, user);
        return true;
    }
}