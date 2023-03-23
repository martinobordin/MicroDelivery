using MicroDelivery.Customers.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroDelivery.Customers.Api.Data
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder
                .Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .OwnsOne(x => x.Address);
        }
    }
}
