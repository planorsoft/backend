using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planor.Application.Duties.Queries.GetActiveDuties;
using Planor.Application.DutyCategories.Commands.CreateDutyCategory;
using Planor.Application.DutyCategories.Commands.DeleteDutyCategory;
using Planor.Application.DutyCategories.Commands.UpdateDutyCategory;
using Planor.Application.DutyCategories.Queries.GetDutyCategories;
using Planor.Application.DutyCategories.Queries.GetDutyCategoryById;
using Planor.Domain.Constants;

namespace Planor.WebUI.Controllers;

[Route("api/duty/categories")]
public class DutyCategoryController : ApiControllerBase
{
    [HttpGet]
    [Authorize(Roles = $"{Roles.Admin},{Roles.DutyRead}")]
    public async Task<ActionResult<List<DutyCategorySmallDto>>> GetDutiesWithPagination([FromQuery] GetDutyCategoriesQuery query)
    {
        return await Mediator.Send(query);
    }
    
    [HttpGet("{id}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.DutyRead}")]
    public async Task<ActionResult<DutyCategoryDto>> GetDutyCategoryById([FromRoute] GetDutyCategoryByIdQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.DutyCreate}")]
    public async Task<ActionResult<int>> Create(CreateDutyCategoryCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.DutyUpdate}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateDutyCategoryCommand command)
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
        await Mediator.Send(new DeleteDutyCategoryCommand { Id = id });

        return NoContent();
    }
}