using MediatR;
using Microsoft.Extensions.Logging;
using Planor.Domain.Events;

namespace Planor.Application.Tags.EventHandlers;

public class TagCreatedEventHandler : INotificationHandler<TagCreatedEvent>
{
    private readonly ILogger<TagCreatedEventHandler> _logger;

    public TagCreatedEventHandler(ILogger<TagCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(TagCreatedEvent notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification;

        _logger.LogInformation("CleanArchitecture Domain Event: {DomainEvent}", domainEvent.GetType().Name);

        return Task.CompletedTask;
    }
}