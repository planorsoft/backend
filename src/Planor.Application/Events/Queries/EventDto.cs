using Planor.Application.Common.Mappings;
using Planor.Application.Users.Queries.GetUser;
using Planor.Domain.Entities;

namespace Planor.Application.Events.Queries;

public class EventDto : IMapFrom<Event>
{
    public int Id { get; set; }
    
    public string Title { get; set; } = null!;
    
    public string? Location { get; set; }

    public string? Description { get; set; }
    
    public long Start { get; set; }
    
    public long? End { get; set; }

    public string Color { get; set; } = null!;
    
    public ICollection<UserSmallDto> Attendee { get; set; } = new List<UserSmallDto>();
}