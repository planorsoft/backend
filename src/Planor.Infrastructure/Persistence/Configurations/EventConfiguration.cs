using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Planor.Domain.Entities;

namespace Planor.Infrastructure.Persistence.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.Property(t => t.Title)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(t => t.Color)
            .HasMaxLength(16)
            .IsRequired();

        builder
            .HasMany(x => x.Attendee)
            .WithMany();

        builder
            .HasMany(x => x.Notifications)
            .WithOne(x => x.Event)
            .HasForeignKey(x => x.EventId)
            .IsRequired();
    }
}