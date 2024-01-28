using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planor.Application.Tenants.Queries.GetTenantByName;

namespace Planor.WebUI.Controllers;

[Route("api/tenants")]
public class TenantController : ApiControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<TenantDto>> GetTenantByName([FromQuery] GetTenantByNameQuery query)
    {
        return await Mediator.Send(query);
    }

}