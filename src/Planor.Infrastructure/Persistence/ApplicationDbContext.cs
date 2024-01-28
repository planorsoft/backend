using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Planor.Application.Common.Interfaces;
using Planor.Domain.Entities;

namespace Planor.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<User>, IApplicationDbContext
{
    private readonly string _tenantId;
    
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ICurrentUserService currentUserService
    ) : base(options)
    {
        _tenantId = currentUserService.TenantId;
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<MailTemplate> MailTemplates => Set<MailTemplate>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Currency> Currencies => Set<Currency>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Duty> Duties => Set<Duty>();
    public DbSet<DutyCategory> DutyCategories => Set<DutyCategory>();
    public DbSet<DutyPriority> DutyPriorities => Set<DutyPriority>();
    public DbSet<DutySize> DutySizes => Set<DutySize>();
    public DbSet<DutyTodo> DutyTodos => Set<DutyTodo>();
    public DbSet<App> Apps => Set<App>();
    public DbSet<Blob> Blobs => Set<Blob>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<Finance> Finances => Set<Finance>();
    public DbSet<FinanceCategory> FinanceCategories => Set<FinanceCategory>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        builder.Entity<Tag>().HasQueryFilter(x => x.TenantId == _tenantId);
        builder.Entity<MailTemplate>().HasQueryFilter(x => x.TenantId == _tenantId);
        builder.Entity<Customer>().HasQueryFilter(x => x.TenantId == _tenantId);
        builder.Entity<Currency>().HasQueryFilter(x => x.TenantId == _tenantId);
        builder.Entity<Project>().HasQueryFilter(x => x.TenantId == _tenantId);
        builder.Entity<Duty>().HasQueryFilter(x => x.TenantId == _tenantId);
        builder.Entity<DutyCategory>().HasQueryFilter(x => x.TenantId == _tenantId);
        builder.Entity<DutyPriority>().HasQueryFilter(x => x.TenantId == _tenantId);
        builder.Entity<DutySize>().HasQueryFilter(x => x.TenantId == _tenantId);
        builder.Entity<DutyTodo>().HasQueryFilter(x => x.TenantId == _tenantId);
        builder.Entity<App>().HasQueryFilter(x => x.TenantId == _tenantId);
        builder.Entity<Blob>().HasQueryFilter(x => x.TenantId == _tenantId);
        builder.Entity<Event>().HasQueryFilter(x => x.TenantId == _tenantId);
        builder.Entity<Notification>().HasQueryFilter(x => x.TenantId == _tenantId);
        builder.Entity<Finance>().HasQueryFilter(x => x.TenantId == _tenantId);
        builder.Entity<FinanceCategory>().HasQueryFilter(x => x.TenantId == _tenantId);

        base.OnModelCreating(builder);
    }
}