using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Constants;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Events.Commands.UpdateEvent;

public class UpdateEventCommand : IRequest
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? Location { get; set; }
    public long Start { get; set; }
    public long? End { get; set; }
    public string Color { get; set; } = null!;
    public ICollection<string> Attendee { get; set; } = new List<string>();
}

public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly ICurrentUserService _currentUserService;
    private readonly INotificationService _notificationService;
    private readonly IConfiguration _configuration;


    public UpdateEventCommandHandler(
        IApplicationDbContext context,
        UserManager<User> userManager,
        ICurrentUserService currentUserService,
        INotificationService notificationService,
        IConfiguration configuration)
    {
        _context = context;
        _userManager = userManager;
        _currentUserService = currentUserService;
        _notificationService = notificationService;
        _configuration = configuration;
    }

    public async Task Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Events
            .Include(x => x.Attendee)
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity is null)
        {
            throw new NotFoundException(nameof(Event), request.Id.ToString());
        }

        entity.Title = request.Title;
        entity.Description = request.Description;
        entity.Location = request.Location;
        entity.Start = request.Start;
        entity.Color = request.Color;
        entity.Attendee.Clear();

        if (request.End.HasValue)
        {
            entity.End = request.End.Value;
        }

        await UpdateAttendee(entity, request.Attendee, cancellationToken);

        var notificationsToRemove = await _context
            .Notifications
            .Where(x => x.EventId == entity.Id)
            .ToListAsync(cancellationToken);

        _notificationService.RemoveRange(notificationsToRemove.Select(x => x.JobId));
        _context.Notifications.RemoveRange(notificationsToRemove);

        var notifications = new List<Notification>();

        foreach (var attendee in entity.Attendee)
        {
            SendNotification(entity, attendee, notifications);
        }

        var currentUser = new User
        {
            Name = _currentUserService.Name,
            Email = _currentUserService.Email
        };

        SendNotification(entity, currentUser, notifications);

        _context.Notifications.AddRange(notifications);

        await _context.SaveChangesAsync(cancellationToken);
    }
    
    private async Task UpdateAttendee(Event entity, ICollection<string> attendeeCollection, CancellationToken cancellationToken)
    {
        foreach (var attendee in attendeeCollection)
        {
            if (_currentUserService.Email == attendee) throw new BadRequestException("Kendini davet edemezsin");

            var user = await _userManager
                .Users
                .FirstOrDefaultAsync(x => x.Email == attendee && x.TenantId == _currentUserService.TenantId,
                    cancellationToken);
            if (user is null) throw new NotFoundException(nameof(User), attendee);

            entity.Attendee.Add(user);
        }
    }
    
    private void SendNotification(Event entity, User attendee, IList<Notification> notifications)
    {
        var data = new
        {
            Name = attendee.Name,
            Message = "Etkinlik 5 dakika sonra başlayacaktır.",
            Link = $"{_configuration.GetSection("ClientUrl")}/login",
        };

        var jobId = _notificationService.Schedule(
            NotificationType.EventTime,
            data,
            attendee,
            DateTimeOffset.FromUnixTimeSeconds(entity.Start).LocalDateTime.AddMinutes(-5));

        if (jobId is not null)
        {
            notifications.Add(new Notification
            {
                Event = entity,
                JobId = jobId
            });
        }
    }
}