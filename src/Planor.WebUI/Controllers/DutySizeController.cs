
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planor.Application.DutySizes.Queries.GetDutySizes;
using Planor.Application.DutySizes.Commands.CreateDutySize;
using Planor.Application.DutySizes.Commands.DeleteDutySize;
using Planor.Application.DutySizes.Commands.UpdateDutySize;
using Planor.Application.DutySizes.Queries;
using Planor.Application.DutySizes.Queries.GetDutySizeById;
using Planor.Domain.Constants;

namespace Planor.WebUI.Controllers;

[Route("api/duty/sizes")]
public class DutySizeController : ApiControllerBase
{
    [HttpGet]
    [Authorize(Roles = $"{Roles.Admin},{Roles.DutyRead}")]
    public async Task<ActionResult<List<DutySizeDto>>> GetDutiesWithPagination([FromQuery] GetDutySizesQuery query)
    {
        return await Mediator.Send(query);
    }
    
    [HttpGet("{id}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.DutyRead}")]
    public async Task<ActionResult<DutySizeDto>> GetDutySizeById([FromRoute] GetDutySizeByIdQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.DutyCreate}")]
    public async Task<ActionResult<int>> Create(CreateDutySizeCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.DutyUpdate}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateDutySizeCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.DutyDelete}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteDutySizeCommand { Id = id });

        return NoContent();
    }
}