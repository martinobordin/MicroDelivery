using MicroDelivery.Orders.Api.Models;
using MongoDB.Driver;

namespace MicroDelivery.Orders.Api.Data
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrdersAsync();
        Task<Order> GetOrderAsync(Guid id);
        Task<long> CountAllOrderAsync();

        Task<Order> CreateOrderAsync(Order order);
        Task<Order> UpdateOrderAsync(Order order);
        Task<bool> DeleteOrderAsync(Guid id);
    }

    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<Order> orders;

        public OrderRepository(IConfiguration configuration)
        {
            var clientSettings = MongoClientSettings.FromConnectionString(configuration.GetValue<string>("MongoDb:ConnectionString"));
            var client = new MongoClient(clientSettings);
            var database = client.GetDatabase(configuration.GetValue<string>("MongoDb:DatabaseName"));

            this.orders = database.GetCollection<Order>(configuration.GetValue<string>("MongoDb:OrdersCollectionName"));
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            return await this.orders.Find(p => true).ToListAsync();
        }

        public async Task<Order> GetOrderAsync(Guid id)
        {
            return await this.orders.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<long> CountAllOrderAsync()
        {
            return await orders.EstimatedDocumentCountAsync();
        }

        public async Task<Order> CreateOrderAsync(Order product)
        {
            await this.orders.InsertOneAsync(product);
            return product;
        }

        public async Task<Order> UpdateOrderAsync(Order product)
        {
            var result = await this.orders.ReplaceOneAsync(p => p.Id == product.Id, product);
            return product;
        }

        public async Task<bool> DeleteOrderAsync(Guid id)
        {
            var result = await this.orders.DeleteOneAsync(p => p.Id == id);
            return result.IsAcknowledged && result.DeletedCount == 1;
        }
    }
}
