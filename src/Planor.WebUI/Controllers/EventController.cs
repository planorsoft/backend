using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planor.Application.Common.Models;
using Planor.Application.Events.Commands.CreateEvent;
using Planor.Application.Events.Commands.DeleteEvent;
using Planor.Application.Events.Commands.UpdateEvent;
using Planor.Application.Events.Queries;
using Planor.Application.Events.Queries.GetEventById;
using Planor.Application.Events.Queries.GetEventsWithPagination;
using Planor.Domain.Constants;

namespace Planor.WebUI.Controllers;

[Route("api/events")]
public class EventController : ApiControllerBase
{
    [HttpGet]
    [Authorize(Roles = $"{Roles.Admin},{Roles.EventRead}")]
    public async Task<ActionResult<PaginatedList<EventSmallDto>>> GetEventsWithPagination([FromQuery] GetEventsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }
    
    [HttpGet("{id}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.EventRead}")]
    public async Task<ActionResult<EventDto>> GetEventById([FromRoute] GetEventByIdQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.EventCreate}")]
    public async Task<ActionResult<int>> Create(CreateEventCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.EventUpdate}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateEventCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.EventDelete}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteEventCommand { Id = id });

        return NoContent();
    }
}