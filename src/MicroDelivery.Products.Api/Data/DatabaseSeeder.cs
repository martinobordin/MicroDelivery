using MicroDelivery.Products.Api.Models;

namespace MicroDelivery.Products.Api.Data
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
            var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
           
            var totalProducts = await productRepository.CountAllProductsAsync();
            if (totalProducts == 0)
            {
                logger.LogInformation($"{nameof(DatabaseSeeder)} is seeding.");
                var sampleProducts = GetSampleProducts();
                foreach (var product in sampleProducts)
                {
                    await productRepository.CreateProductAsync(product);
                    logger.LogInformation($"Product {product.Name} created.");
                }
            }
        }

        private static IEnumerable<Product> GetSampleProducts()
        {
            yield return new Product { Id = 1, Name = "Tomato salad", Price = 8, Categories = new[] { "Salad", "Vegan", "Food" } };
            yield return new Product { Id = 2, Name = "Margherita", Price = 6, Categories = new[] { "Pizza", "Food" } };
            yield return new Product { Id = 3, Name = "Marinara", Price = 5, Categories = new[] { "Pizza", "Food" } };
            yield return new Product { Id = 4, Name = "Water", Price = 2, Categories = new[] { "Beverage", "Non alcoholic" } };
            yield return new Product { Id = 5, Name = "Tea", Price = 3, Categories = new[] { "Beverage", "Non alcoholic" } };
            yield return new Product { Id = 6, Name = "Bier", Price = 4, Categories = new[] { "Beverage", "Alcoholic" } };
        }
    }
}
