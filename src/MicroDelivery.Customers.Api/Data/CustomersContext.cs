using MicroDelivery.Customers.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroDelivery.Customers.Api.Data
{
    public class CustomersContext : DbContext
    {
        public CustomersContext(DbContextOptions<CustomersContext> options)
              : base(options)
        {
        }

        public DbSet<Customer> Customers{ get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomersContext).Assembly);
        }
    }
}
