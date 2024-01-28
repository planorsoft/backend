using MediatR;
using Microsoft.AspNetCore.Identity;
using Planor.Application.Roles.Commands.UpdateRole;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Roles.Commands.AddToRole;

public class AddToRoleCommand : IRequest<bool>
{
    public string Email { get; set; } = null!;

    public string Role { get; set; } = null!;
}

public class AddToRoleCommandHandler : IRequestHandler<AddToRoleCommand, bool>
{
    private readonly UserManager<User> _userManager;

    public AddToRoleCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> Handle(AddToRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null) throw new NotFoundException(nameof(User), request.Email);

        var isInRole = await _userManager.IsInRoleAsync(user, request.Role);

        if (isInRole) throw new BadRequestException("Kullanıcı zaten bu role sahip");
 
        await _userManager.AddToRoleAsync(user, request.Role);

        return true;
    }
}