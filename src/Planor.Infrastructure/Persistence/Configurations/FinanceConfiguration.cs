using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Planor.Domain.Entities;

namespace Planor.Infrastructure.Persistence.Configurations;

public class FinanceConfiguration : IEntityTypeConfiguration<Finance>
{
    public void Configure(EntityTypeBuilder<Finance> builder)
    {
        builder
            .HasOne(x => x.Project)
            .WithMany(x => x.Finances)
            .IsRequired(false);
        
        builder
            .HasOne(x => x.Customer)
            .WithMany(x => x.Finances)
            .IsRequired(false);

        builder
            .HasOne(x => x.Category)
            .WithMany(x => x.Finances)
            .IsRequired();
    }
}