using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Planor.Domain.Entities;

namespace Planor.Infrastructure.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder
            .HasMany(x => x.Tags)
            .WithMany()
            .UsingEntity("customer_join_tag");
        
        builder
            .HasMany(x => x.Projects)
            .WithOne(x => x.Customer)
            .HasForeignKey(x => x.CustomerId)
            .IsRequired(false);
        
        builder
            .HasMany(x => x.Contacts)
            .WithOne(x => x.Customer)
            .HasForeignKey(x => x.CustomerId)
            .IsRequired(false);
    }
}