using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planor.Application.Duties.Commands.CreateDuty;
using Planor.Application.Duties.Commands.DeleteDuty;
using Planor.Application.Duties.Commands.UpdateDuty;
using Planor.Application.Duties.Commands.UpdateDutyOrders;
using Planor.Application.Duties.Queries.GetActiveDuties;
using Planor.Application.Duties.Queries.GetDutyById;
using Planor.Domain.Constants;

namespace Planor.WebUI.Controllers;

[Route("api/duties")]
public class DutyController : ApiControllerBase
{
    [HttpGet]
    [Authorize(Roles = $"{Roles.Admin},{Roles.DutyRead}")]
    public async Task<ActionResult<List<DutySmallDto>>> GetDutiesWithPagination([FromQuery] GetActiveDutiesQuery query)
    {
        return await Mediator.Send(query);
    }
    
    [HttpGet("{id}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.DutyRead}")]
    public async Task<ActionResult<DutyDto>> GetDutyById([FromRoute] GetDutyByIdQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.DutyCreate}")]
    public async Task<ActionResult<int>> Create(CreateDutyCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.DutyUpdate}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateDutyCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }
    
    [HttpPut("orders")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.DutyUpdate}")]
    public async Task<ActionResult> Update([FromBody] IList<UpdateDutyOrdersDto> dutyOrders)
    {
        var command = new UpdateDutyOrdersCommand(dutyOrders);
        await Mediator.Send(command);

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.DutyDelete}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteDutyCommand { Id = id });

        return NoContent();
    }
}