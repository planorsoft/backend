using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest
{
    public string Email { get; set; } = null!;
    public string? Name { get; set; }
    public string? Username { get; set; }
    public string? OldPassword { get; set; }
    public string? NewPassword { get; set; }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly ICurrentUserService _currentUserService;

    public UpdateUserCommandHandler(IApplicationDbContext context, UserManager<User> userManager,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _userManager = userManager;
        _currentUserService = currentUserService;
    }

    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x =>
            x.Email == request.Email && x.TenantId == _currentUserService.TenantId, cancellationToken: cancellationToken);
        
        if (user is null) throw new NotFoundException(nameof(User), request.Email);

        if (!string.IsNullOrEmpty(request.Name))
        {
            user.Name = request.Name;
        }
        
        if (!string.IsNullOrEmpty(request.Username))
        {
            user.UserName = request.Username;
        }

        if (!(string.IsNullOrEmpty(request.NewPassword) && string.IsNullOrEmpty(request.OldPassword)))
        {
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.OldPassword);
            if (!isPasswordValid) throw new BadRequestException("Parola yanlış");
            
            await _userManager.ChangePasswordAsync(user, request.OldPassword!, request.NewPassword!);
        }

        await _userManager.UpdateAsync(user);
    }
}