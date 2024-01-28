using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planor.Application.Common.Models;
using Planor.Application.Projects.Commands.CreateProject;
using Planor.Application.Projects.Commands.DeleteProject;
using Planor.Application.Projects.Commands.UpdateProject;
using Planor.Application.Projects.Queries.GetProjectById;
using Planor.Application.Projects.Queries.GetProjectsWithPagination;
using Planor.Domain.Constants;

namespace Planor.WebUI.Controllers;

[Route("api/projects")]
public class ProjectController : ApiControllerBase
{
    [HttpGet]
    [Authorize(Roles = $"{Roles.Admin},{Roles.ProjectRead}")]
    public async Task<ActionResult<PaginatedList<ProjectSmallDto>>> GetProjectsWithPagination([FromQuery] GetProjectsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.ProjectRead}")]
    public async Task<ActionResult<ProjectDto>> GetProjectById([FromRoute] GetProjectByIdQuery query)
    {
        return await Mediator.Send(query);
    }


    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.ProjectCreate}")]
    public async Task<ActionResult<int>> Create(CreateProjectCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.ProjectUpdate}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateProjectCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.ProjectDelete}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteProjectCommand { Id = id });

        return NoContent();
    }
}