using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Application.Identity.Commands.Forgot;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Identity.Commands.ForgotConfirm;

public class ForgotConfirmCommand : IRequest<bool>
{
    public string Email { get; set; } = null!;
    public string Token { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Tenant { get; set; } = null!;
}

public class ForgotConfirmCommandHandler : IRequestHandler<ForgotConfirmCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly INotificationService _notificationService;
    private readonly IIdentityService _identityService;
    private readonly ICurrentUserService _currentUserService;

    public ForgotConfirmCommandHandler(
        IApplicationDbContext context,
        UserManager<User> userManager,
        INotificationService notificationService,
        IIdentityService identityService,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _userManager = userManager;
        _notificationService = notificationService;
        _identityService = identityService;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(ForgotConfirmCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(x => x.Email == request.Email && x.TenantId == request.Tenant, cancellationToken: cancellationToken);

        if (user is null) throw new NotFoundException(nameof(User), request.Email);

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

        if (!result.Succeeded) throw new BadRequestException("Şifre sıfırlanamadı");
        
        return true;
    }
}


