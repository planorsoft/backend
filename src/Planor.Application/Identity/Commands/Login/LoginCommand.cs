using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Application.Identity.Commands.Register;
using Planor.Domain.Constants;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Identity.Commands.Login;

public class LoginCommand : IRequest<LoginDto>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Tenant { get; set; } = null!;
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginDto>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly INotificationService _notificationService;
    private readonly IIdentityService _identityService;


    public LoginCommandHandler(
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

    public async Task<LoginDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(x => x.Email == request.Email && x.TenantId == request.Tenant);
        if (user is null) throw new NotFoundException(nameof(User), request.Email);

        var isPasswordInvalid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordInvalid) throw new BadRequestException("Parola yanlış");

        var isInTenant = user.TenantId == request.Tenant;
        if (!isInTenant) throw new NotFoundException(nameof(Tenant), request.Tenant);

        if (!user.EmailConfirmed) throw new BadRequestException("Lütfen ilk önce mail adresini onayla");

        var token = await _identityService.GenerateToken(user);

        return new LoginDto()
        {
            AccessToken = token
        };
    }
}
