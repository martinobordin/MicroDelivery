using MicroDelivery.Customers.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroDelivery.Customers.Api.Data
{
    public class CustomerContext : DbContext
    {
        public CustomerContext(DbContextOptions<CustomerContext> options)
              : base(options)
        {
        }

        public DbSet<Customer> Customers{ get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerContext).Assembly);
        }
    }
}
