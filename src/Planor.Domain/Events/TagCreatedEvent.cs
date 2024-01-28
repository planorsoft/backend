using Planor.Domain.Common;
using Planor.Domain.Entities;

namespace Planor.Domain.Events;

public class TagCreatedEvent : BaseEvent
{
    public TagCreatedEvent(Tag tag)
    {
        Tag = tag;
    }

    public Tag Tag { get; }
}