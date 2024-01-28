using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planor.Application.FinanceCategories.Commands.CreateFinanceCategory;
using Planor.Application.FinanceCategories.Commands.DeleteFinanceCategory;
using Planor.Application.FinanceCategories.Commands.UpdateFinanceCategory;
using Planor.Application.FinanceCategories.Queries;
using Planor.Application.FinanceCategories.Queries.GetFinanceCategories;
using Planor.Application.FinanceCategories.Queries.GetFinanceCategoryById;
using Planor.Domain.Constants;

namespace Planor.WebUI.Controllers;

[Route("api/finance/categories")]
public class FinanceCategoriesController : ApiControllerBase
{
    [HttpGet]
    [Authorize(Roles = $"{Roles.Admin},{Roles.FinanceRead}")]
    public async Task<ActionResult<List<FinanceCategoryDto>>> GetFinanceCategories([FromQuery] GetFinanceCategoriesQuery query)
    {
        return await Mediator.Send(query);
    }
    
    [HttpGet("{id}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.FinanceRead}")]
    public async Task<ActionResult<FinanceCategoryDto>> GetFinanceCategoryById([FromRoute] GetFinanceCategoryByIdQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.FinanceCreate}")]
    public async Task<ActionResult<int>> Create(CreateFinanceCategoryCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.FinanceUpdate}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateFinanceCategoryCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.FinanceDelete}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteFinanceCategoryCommand { Id = id });

        return NoContent();
    }
}