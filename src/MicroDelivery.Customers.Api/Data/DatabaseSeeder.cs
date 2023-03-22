using MicroDelivery.Customers.Api.Models;

namespace MicroDelivery.Customers.Api.Data
{
    public class DatabaseSeeder : IHostedService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<DatabaseSeeder> logger;

        public DatabaseSeeder(IServiceProvider serviceProvider, ILogger<DatabaseSeeder> logger)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await SeedDatabaseIfEmpty();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task SeedDatabaseIfEmpty()
        {
            logger.LogInformation($"{nameof(DatabaseSeeder)} is working.");

            using IServiceScope scope = serviceProvider.CreateScope();

            var customerContext = scope.ServiceProvider.GetService<CustomerContext>();
            await customerContext.Database.EnsureCreatedAsync();

            var customerRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();

            var totalCustomers = await customerRepository.CountAllCustomersAsync();
            if (totalCustomers == 0)
            {
                logger.LogInformation($"{nameof(DatabaseSeeder)} is seeding.");
                var sampleCustomers = GetSampleCustomers();
                foreach (var customer in sampleCustomers)
                {
                    await customerRepository.CreateCustomerAsync(customer);
                    logger.LogInformation($"Customer {customer.FirstName} {customer.LastName} created.");
                }
            }
        }

        private static IEnumerable<Customer> GetSampleCustomers()
        {
            yield return new Customer { FirstName = "Joe", LastName = "Doe", Address = new Address { AddressLine = "Main street 1", ZipCode = "123", State = "FL" } };
            yield return new Customer { FirstName = "Jane", LastName = "Smith", Address = new Address { AddressLine = "Other road", ZipCode = "456", State = "TX" } };
        }
    }
}
