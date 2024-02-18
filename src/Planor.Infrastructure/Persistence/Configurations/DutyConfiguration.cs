using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Planor.Domain.Entities;

namespace Planor.Infrastructure.Persistence.Configurations;

public class DutyConfiguration : IEntityTypeConfiguration<Duty>
{
    public void Configure(EntityTypeBuilder<Duty> builder)
    {
        builder.Property(t => t.Title)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(t => t.Description)
            .HasMaxLength(4096);

        builder
            .HasMany(x => x.Helpers)
            .WithMany();
        
        builder
            .HasMany(x => x.Todos)
            .WithOne(x => x.Duty)
            .HasForeignKey(x => x.DutyId)
            .IsRequired();

        builder
            .HasMany(x => x.Tags)
            .WithMany()
            .UsingEntity("duty_tag_join");
    }
}