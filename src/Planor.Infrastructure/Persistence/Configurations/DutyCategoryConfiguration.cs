using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Planor.Domain.Entities;

namespace Planor.Infrastructure.Persistence.Configurations;

public class DutyCategoryConfiguration : IEntityTypeConfiguration<DutyCategory>
{
    public void Configure(EntityTypeBuilder<DutyCategory> builder)
    {
        builder
            .HasMany(x => x.Duties)
            .WithOne(x => x.Category)
            .HasForeignKey(x => x.CategoryId)
            .IsRequired();

    }
}