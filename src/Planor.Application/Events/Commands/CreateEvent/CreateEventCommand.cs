using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Constants;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Application.Events.Commands.CreateEvent;

public class CreateEventCommand : IRequest<int>
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? Location { get; set; }
    public long Start { get; set; }
    public long? End { get; set; }
    public string Color { get; set; } = null!;
    public ICollection<string> Attendee { get; set; } = new List<string>();
}

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly INotificationService _notificationService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IConfiguration _configuration;

    public CreateEventCommandHandler(
        IApplicationDbContext context,
        UserManager<User> userManager,
        INotificationService notificationService,
        ICurrentUserService currentUserService,
        IConfiguration configuration)
    {
        _context = context;
        _userManager = userManager;
        _notificationService = notificationService;
        _currentUserService = currentUserService;
        _configuration = configuration;
    }

    public async Task<int> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var entity = new Event
        {
            Title = request.Title,
            Description = request.Description,
            Location = request.Location,
            Start = request.Start,
            Color = request.Color,
            Attendee = new List<User>()
        };

        if (request.End.HasValue)
        {
            entity.End = request.End.Value;
        }

        await UpdateAttendee(entity, request.Attendee, cancellationToken);

        _context.Events.Add(entity);

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

        return entity.Id;
    }

    private async Task UpdateAttendee(Event entity, ICollection<string> attendeeCollection, CancellationToken cancellationToken)
    {
        entity.Attendee = new List<User>();

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