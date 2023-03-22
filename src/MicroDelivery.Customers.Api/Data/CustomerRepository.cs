using MicroDelivery.Customers.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroDelivery.Customers.Api.Data
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetCustomersAsync();
        Task<Customer?> GetCustomerAsync(int id);
        Task<long> CountAllCustomersAsync();

        Task<Customer> CreateCustomerAsync(Customer customer);
        Task<Customer> UpdateCustomerAsync(Customer customer);
        Task<bool> DeleteCustomerAsync(int id);
    }
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerContext customerContext;

        public CustomerRepository(CustomerContext customerContext)
        {
            this.customerContext = customerContext;
        }
        public async Task<IEnumerable<Customer>> GetCustomersAsync()
        {
            return await this.customerContext.Customers.ToListAsync();
        }

        public async Task<Customer?> GetCustomerAsync(int id)
        {
            return await this.customerContext.Customers.FindAsync(id);
        }
        public async Task<long> CountAllCustomersAsync()
        {
            return await this.customerContext.Customers.LongCountAsync();
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            await customerContext.Customers.AddAsync(customer);
            await customerContext.SaveChangesAsync();
            return customer;
        }
        public async Task<Customer> UpdateCustomerAsync(Customer customer)
        {
            customerContext.Customers.Update(customer);
            await customerContext.SaveChangesAsync();
            return customer;
        }
        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var rowsAffected = await customerContext.Customers.Where(c => c.Id == id).ExecuteDeleteAsync();
            return rowsAffected > 0;
        }
    }
}
