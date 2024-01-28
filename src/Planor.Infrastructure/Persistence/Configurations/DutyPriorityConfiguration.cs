using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Planor.Domain.Entities;

namespace Planor.Infrastructure.Persistence.Configurations;

public class DutyPriorityConfiguration : IEntityTypeConfiguration<DutyPriority>
{
    public void Configure(EntityTypeBuilder<DutyPriority> builder)
    {
        builder.Property(t => t.Name)
            .HasMaxLength(64)
            .IsRequired();
        
        builder
            .HasMany(x => x.Duties)
            .WithOne(x => x.Priority)
            .HasForeignKey(x => x.PriorityId)
            .IsRequired(false);
    }
}