using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Roles.Queries.GetRoles;

public class GetRolesQuery : IRequest<RoleDto>
{
    public string Email { get; set; } = null!;
}

public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, RoleDto>
{
    private readonly UserManager<User> _userManager;

    public GetRolesQueryHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<RoleDto> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null) throw new NotFoundException(nameof(User), request.Email);

        var roles = await _userManager.GetRolesAsync(user);

        roles = roles.OrderBy(x => x).ToList();

        var rolesDto = new RoleDto()
        {
            Roles = roles
        };

        return rolesDto;
    }
}