using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planor.Application.Common.Models;
using Planor.Application.Apps.Commands.CreateApp;
using Planor.Application.Apps.Commands.DeleteApp;
using Planor.Application.Apps.Commands.UpdateApp;
using Planor.Application.Apps.Queries;
using Planor.Application.Apps.Queries.GetAppById;
using Planor.Application.Apps.Queries.GetApps;
using Planor.Application.Apps.Queries.GetCurrentApp;
using Planor.Domain.Constants;

namespace Planor.WebUI.Controllers;


[Route("api/apps")]
public class AppController : ApiControllerBase
{
    [HttpGet]
    [Authorize(Roles = $"{Roles.Admin}")]
    public async Task<ActionResult<List<AppDto>>> GetApps([FromQuery] GetAppsQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = $"{Roles.Admin}")]
    public async Task<ActionResult<AppDto>> GetAppById([FromRoute] GetAppByIdQuery query)
    {
        return await Mediator.Send(query);
    }
    
    [HttpGet("current")]
    [Authorize]
    public async Task<ActionResult<AppDto>> GetCurrentApp([FromRoute] GetCurrentAppQuery query)
    {
        return await Mediator.Send(query);
    }


    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Manager}")]
    public async Task<ActionResult<int>> Create(CreateAppCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Manager}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateAppCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Manager}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteAppCommand { Id = id });

        return NoContent();
    }
}