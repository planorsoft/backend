using MediatR;
using Microsoft.AspNetCore.Identity;
using Planor.Application.Common.Interfaces;
using Planor.Application.Tags.Commands.UpdateTag;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Roles.Commands.UpdateRole;

public class UpdateRoleCommand : IRequest<bool>
{
    public string Email { get; set; } = null!;

    public List<string> Roles { get; set; }
}

public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, bool>
{
    private readonly UserManager<User> _userManager;

    public UpdateRoleCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null) throw new NotFoundException(nameof(User), request.Email);

        foreach (var role in request.Roles)
        {
            if (await _userManager.IsInRoleAsync(user, role))
            {
                
            }
        }
        
        
        return true;
    }
}