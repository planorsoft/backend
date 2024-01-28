using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planor.Application.Common.Models;
using Planor.Application.Finances.Commands.CreateFinance;
using Planor.Application.Finances.Commands.DeleteFinance;
using Planor.Application.Finances.Commands.UpdateFinance;
using Planor.Application.Finances.Queries;
using Planor.Application.Finances.Queries.GetFinanceById;
using Planor.Application.Finances.Queries.GetFinanceWithPagination;
using Planor.Domain.Constants;

namespace Planor.WebUI.Controllers;

[Route("api/finances")]
public class FinancesController : ApiControllerBase
{
    [HttpGet]
    [Authorize(Roles = $"{Roles.Admin},{Roles.FinanceRead}")]
    public async Task<ActionResult<PaginatedList<FinanceSmallDto>>> GetFinancesWithPagination([FromQuery] GetFinancesWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }
    
    [HttpGet("{id}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.FinanceRead}")]
    public async Task<ActionResult<FinanceDto>> GetFinanceById([FromRoute] GetFinanceByIdQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.FinanceCreate}")]
    public async Task<ActionResult<int>> Create(CreateFinanceCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.FinanceUpdate}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateFinanceCommand command)
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
        await Mediator.Send(new DeleteFinanceCommand { Id = id });

        return NoContent();
    }
}