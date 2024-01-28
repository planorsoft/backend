using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planor.Application.Common.Models;
using Planor.Application.Tags.Commands.CreateTag;
using Planor.Application.Tags.Commands.DeleteTag;
using Planor.Application.Tags.Commands.UpdateTag;
using Planor.Application.Tags.Queries.GetTagsWithPagination;
using Planor.Domain.Constants;

namespace Planor.WebUI.Controllers;

[Route("api/tags")]
public class TagsController : ApiControllerBase
{
    [HttpGet]
    [Authorize(Roles = $"{Roles.Admin},{Roles.TagRead}")]
    public async Task<ActionResult<PaginatedList<TagDto>>> GetTagsWithPagination([FromQuery] GetTagsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.TagCreate}")]
    public async Task<ActionResult<int>> Create(CreateTagCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.TagUpdate}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateTagCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.TagDelete}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteTagCommand { Id = id });

        return NoContent();
    }
}