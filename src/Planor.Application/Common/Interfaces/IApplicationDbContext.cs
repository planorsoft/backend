using Microsoft.EntityFrameworkCore;
using Planor.Domain.Entities;

namespace Planor.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<Tenant> Tenants { get; }
    DbSet<Tag> Tags { get; }
    DbSet<MailTemplate> MailTemplates { get; }
    DbSet<Customer> Customers { get; }
    DbSet<Currency> Currencies { get; }
    DbSet<Project> Projects { get; }
    DbSet<Duty> Duties { get; }
    DbSet<DutyCategory> DutyCategories { get; }
    DbSet<DutyPriority> DutyPriorities { get; }
    DbSet<DutySize> DutySizes { get; }
    DbSet<DutyTodo> DutyTodos { get; }
    DbSet<App> Apps { get; }
    DbSet<Blob> Blobs { get; }
    DbSet<Event> Events { get; }
    DbSet<Notification> Notifications { get; }
    DbSet<Finance> Finances { get; }
    DbSet<FinanceCategory>FinanceCategories { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}