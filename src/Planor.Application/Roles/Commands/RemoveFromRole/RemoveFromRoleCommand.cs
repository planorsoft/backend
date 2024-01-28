using MediatR;
using Microsoft.AspNetCore.Identity;
using Planor.Application.Roles.Commands.AddToRole;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Roles.Commands.RemoveFromRole;

public class RemoveFromRoleCommand : IRequest<bool>
{
    public string Email { get; set; } = null!;

    public string Role { get; set; } = null!;
}

public class RemoveFromRoleCommandHandler : IRequestHandler<RemoveFromRoleCommand, bool>
{
    private readonly UserManager<User> _userManager;

    public RemoveFromRoleCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> Handle(RemoveFromRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null) throw new NotFoundException(nameof(User), request.Email);

        var isInRole = await _userManager.IsInRoleAsync(user, request.Role);

        if (!isInRole) throw new BadRequestException("Kullanıcı zaten bu role sahip değil");
 
        await _userManager.RemoveFromRoleAsync(user, request.Role);

        return true;
    }
}