using MediatR;
using Microsoft.EntityFrameworkCore;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Events.Commands.DeleteEvent;

public class DeleteEventCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly INotificationService _notificationService;

    public DeleteEventCommandHandler(IApplicationDbContext context, INotificationService notificationService)
    {
        _context = context;
        _notificationService = notificationService;
    }

    public async Task Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Events
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity is null) throw new NotFoundException(nameof(Event), request.Id.ToString());

        entity.Attendee = new List<User>();
        
        var notificationsToRemove = await _context
            .Notifications
            .Where(x => x.EventId == entity.Id)
            .ToListAsync(cancellationToken);

        _notificationService.RemoveRange(notificationsToRemove.Select(x => x.JobId));
        _context.Notifications.RemoveRange(notificationsToRemove);

        _context.Events.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}