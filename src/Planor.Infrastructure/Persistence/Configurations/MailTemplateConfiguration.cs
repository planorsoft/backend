using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Planor.Domain.Entities;

namespace Planor.Infrastructure.Persistence.Configurations;

public class MailTemplateConfiguration : IEntityTypeConfiguration<MailTemplate>
{
    public void Configure(EntityTypeBuilder<MailTemplate> builder)
    {
        builder.Property(t => t.Title)
            .HasMaxLength(64)
            .IsRequired();
        
        builder.Property(t => t.Slug)
            .HasMaxLength(64)
            .IsRequired();
        
        builder.Property(t => t.Body)
            .IsRequired();
        
    }
}