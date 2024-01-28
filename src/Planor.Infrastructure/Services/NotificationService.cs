using HandlebarsDotNet;
using Hangfire;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MimeKit;
using MimeKit.Text;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Constants;
using Planor.Domain.Entities;
using Planor.Domain.Exceptions;

namespace Planor.Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly IConfiguration _configuration;
    private readonly IApplicationDbContext _context;

    public NotificationService(
        IConfiguration configuration,
        IApplicationDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    public bool RemoveRange(IEnumerable<string> jobIdList)
    {
        var result = true;

        foreach (var jobId in jobIdList)
        {
            var remove = Remove(jobId);

            if (remove is false)
            {
                result = remove;
            }
        }

        return result;
    }

    public bool Remove(string jobId)
    {
        return BackgroundJob.Delete(jobId);
    }

    public string? Schedule(string notificationType, object data, User user, DateTimeOffset delay)
    {
        if (user.Email is null) throw new ArgumentException("User.Email must not be null for sending notification");

        var jobId = BackgroundJob.Schedule(() => SendMailAsync(notificationType, data, user.Email), delay);

        return jobId;
    }

    public void Send(string notificationType, object data, User user)
    {
        if (user.Email is null) throw new ArgumentException("User.Email must not be null for sending notification");

        BackgroundJob.Enqueue(() => SendMailAsync(notificationType, data, user.Email));
    }

    public async Task SendMailAsync(string notificationType, object data, string to)
    {
        if (!NotificationType.List.Contains(notificationType))
            throw new ArgumentException("MailService.SendMail must be feed with NotificationType.");

        var host = _configuration.GetValue<string>("Mail:Host");
        var port = _configuration.GetValue<int>("Mail:Port");
        var user = _configuration.GetValue<string>("Mail:User");
        var password = _configuration.GetValue<string>("Mail:Password");
        var from = _configuration.GetValue<string>("Mail:From");

        var mailTemplate = await _context
            .MailTemplates
            .FirstOrDefaultAsync(x => x.Slug == notificationType);

        if (mailTemplate is null)
        {
            mailTemplate = await _context
                .MailTemplates
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Slug == notificationType && x.TenantId == "planor");

            if (mailTemplate is null) throw new NotFoundException(nameof(MailTemplate), notificationType);
        }

        var template = Handlebars.Compile(mailTemplate.Body);

        // create message
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(from));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = mailTemplate.Title;
        email.Body = new TextPart(TextFormat.Html) { Text = template(data) };

        // send email
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(host, port, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(user, password);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}