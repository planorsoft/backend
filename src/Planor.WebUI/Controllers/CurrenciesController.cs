using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planor.Application.Currencies.Commands.CreateCurrency;
using Planor.Application.Currencies.Commands.DeleteCurrency;
using Planor.Application.Currencies.Commands.UpdateCurrency;
using Planor.Application.Currencies.Queries.GetCurrencies;
using Planor.Domain.Constants;

namespace Planor.WebUI.Controllers;



[Route("api/currencies")]
public class CurrenciesController : ApiControllerBase
{
    [HttpGet]
    [Authorize(Roles = $"{Roles.Admin},{Roles.CurrencyRead}")]
    public async Task<ActionResult<List<CurrencyDto>>> Get([FromQuery] GetCurrenciesQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.CurrencyCreate}")]
    public async Task<ActionResult<int>> Create(CreateCurrencyCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = $"{Roles.Admin}, {Roles.CurrencyUpdate}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateCurrencyCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.CurrencyDelete}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteCurrencyCommand { Id = id });

        return NoContent();
    }
}