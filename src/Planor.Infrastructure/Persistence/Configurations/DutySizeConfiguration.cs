using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Planor.Domain.Entities;

namespace Planor.Infrastructure.Persistence.Configurations;

public class DutySizeConfiguration : IEntityTypeConfiguration<DutySize>
{
    public void Configure(EntityTypeBuilder<DutySize> builder)
    {
        builder.Property(t => t.Name)
            .HasMaxLength(64)
            .IsRequired();
        
        builder
            .HasMany(x => x.Duties)
            .WithOne(x => x.Size)
            .HasForeignKey(x => x.SizeId)
            .IsRequired(false);
    }
}