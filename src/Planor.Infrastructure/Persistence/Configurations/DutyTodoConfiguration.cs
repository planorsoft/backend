using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Planor.Domain.Entities;

namespace Planor.Infrastructure.Persistence.Configurations;

public class DutyTodoConfiguration : IEntityTypeConfiguration<DutyTodo>
{
    public void Configure(EntityTypeBuilder<DutyTodo> builder)
    {
        builder.Property(t => t.Content)
            .HasMaxLength(512)
            .IsRequired();
       
    }
}