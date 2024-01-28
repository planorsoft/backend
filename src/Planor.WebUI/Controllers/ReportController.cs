using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planor.Application.Common.Models;
using Planor.Application.Projects.Queries.GetProjectById;
using Planor.Application.Projects.Queries.GetProjectsWithPagination;
using Planor.Application.Reports.Queries.GetProjectsSummary;
using Planor.Domain.Constants;

namespace Planor.WebUI.Controllers;

[Route("api/reports")]
public class ReportController : ApiControllerBase
{
    [HttpGet("projects-summary")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.ProjectRead}")]
    public async Task<ActionResult<GetProjectsSummaryDto>> GetProjectsSummary([FromQuery] GetProjectsSummaryQuery query)
    {
        return await Mediator.Send(query);
    }

}