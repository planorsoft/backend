using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planor.Application.Common.Models;
using Planor.Application.Projects.Queries.GetProjectById;
using Planor.Application.Projects.Queries.GetProjectsWithPagination;
using Planor.Application.Reports.Queries.GetCustomersSummary;
using Planor.Application.Reports.Queries.GetDutiesSummary;
using Planor.Application.Reports.Queries.GetFinanceSummary;
using Planor.Application.Reports.Queries.GetProjectsSummary;
using Planor.Domain.Constants;

namespace Planor.WebUI.Controllers;

[Route("api/reports")]
public class ReportController : ApiControllerBase
{
    [HttpGet("projects")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.ProjectRead}")]
    public async Task<ActionResult<GetProjectsSummaryDto>> GetProjectsSummary([FromQuery] GetProjectsSummaryQuery query)
    {
        return await Mediator.Send(query);
    }
    
    [HttpGet("customers")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.CustomerRead}")]
    public async Task<ActionResult<GetCustomersSummaryDto>> GetCustomersSummary([FromQuery] GetCustomersSummaryQuery query)
    {
        return await Mediator.Send(query);
    }
    
    [HttpGet("finances")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.FinanceRead}")]
    public async Task<ActionResult<GetFinanceSummaryDto>> GetCustomersSummary([FromQuery] GetFinanceSummaryQuery query)
    {
        return await Mediator.Send(query);
    }
    
    [HttpGet("duties")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.DutyRead}")]
    public async Task<ActionResult<GetDutiesSummaryDto>> GetCustomersSummary([FromQuery] GetDutiesSummaryQuery query)
    {
        return await Mediator.Send(query);
    }

}