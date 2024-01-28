using Planor.Domain.Common;

namespace Planor.Application.Common.Interfaces;

public interface IDomainEventService
{
    Task Publish(BaseEvent baseEvent);
}