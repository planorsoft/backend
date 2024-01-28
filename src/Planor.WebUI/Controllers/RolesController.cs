using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planor.Application.Roles.Commands.AddToRole;
using Planor.Application.Roles.Commands.RemoveFromRole;
using Planor.Application.Roles.Queries.GetRoles;
using Planor.Domain.Constants;

namespace Planor.WebUI.Controllers;

[Route("api/roles")]
public class RolesController : ApiControllerBase
{
    
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<RoleDto>> GetRoles([FromQuery] GetRolesQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpDelete]
    [Authorize(Roles = $"{Roles.Manager},{Roles.Admin}")]
    public async Task<ActionResult<bool>> RemoveFromRole(RemoveFromRoleCommand command)
    {
        return await Mediator.Send(command);
    }
    
    
    [HttpPost]
    [Authorize(Roles = $"{Roles.Manager},{Roles.Admin}")]
    public async Task<ActionResult<bool>> AddToRole(AddToRoleCommand command)
    {
        return await Mediator.Send(command);
    }
}