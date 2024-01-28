using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Application.Identity.Commands.Login;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Identity.Commands.ConfirmEmail;

public class ConfirmEmailCommand : IRequest<bool>
{
    public string Email { get; set; } = null!;
    public string Token { get; set; } = null!;
    public string Tenant { get; set; } = null!;
}

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly INotificationService _notificationService;
    private readonly IIdentityService _identityService;


    public ConfirmEmailCommandHandler(
        IApplicationDbContext context,
        UserManager<User> userManager,
        INotificationService notificationService,
        IIdentityService identityService)
    {
        _context = context;
        _userManager = userManager;
        _notificationService = notificationService;
        _identityService = identityService;
    }

    public async Task<bool> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(x => x.Email == request.Email && x.TenantId == request.Tenant);
        if (user is null) throw new NotFoundException(nameof(User), request.Email);

        var confirmEmail = await _identityService.ConfirmMailConfirmationToken(user, request.Token);

        if (!confirmEmail) throw new BadRequestException("Token veya email hatalı");

        return true;
    }
}