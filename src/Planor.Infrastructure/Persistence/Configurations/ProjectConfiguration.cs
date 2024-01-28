using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Planor.Domain.Entities;

namespace Planor.Infrastructure.Persistence.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.Property(t => t.Title)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(t => t.Description)
            .HasMaxLength(4096);
        
        builder
            .HasMany(x => x.Tags)
            .WithMany()
            .UsingEntity("project_join_tag");
        
        builder
            .HasMany(x => x.Duties)
            .WithOne(x => x.Project)
            .HasForeignKey(x => x.ProjectId)
            .IsRequired();

        builder
            .HasOne(x => x.Currency)
            .WithMany(x => x.Projects)
            .HasForeignKey(x => x.CurrencyId)
            .IsRequired(false);
    }
}