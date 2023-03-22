using MicroDelivery.Products.Api.Models;
using MongoDB.Driver;

namespace MicroDelivery.Products.Api.Data
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product> GetProductAsync(int id);
        Task<long> CountAllProductsAsync();

        Task<Product> CreateProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int id);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> products;

        public ProductRepository(IConfiguration configuration)
        {
            var clientSettings = MongoClientSettings.FromConnectionString(configuration.GetValue<string>("MongoDb:ConnectionString"));
            var client = new MongoClient(clientSettings);
            var database = client.GetDatabase(configuration.GetValue<string>("MongoDb:DatabaseName"));

            this.products = database.GetCollection<Product>(configuration.GetValue<string>("MongoDb:ProductsCollectionName"));
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await this.products.Find(p => true).ToListAsync();
        }

        public async Task<Product> GetProductAsync(int id)
        {
            return await this.products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<long> CountAllProductsAsync()
        {
            return await products.EstimatedDocumentCountAsync();
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            await this.products.InsertOneAsync(product);
            return product;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            var result = await this.products.ReplaceOneAsync(p => p.Id == product.Id, product);
            return product;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var result = await this.products.DeleteOneAsync(p => p.Id == id);
            return result.IsAcknowledged && result.DeletedCount == 1;
        }
    }
}
