﻿using MicroDelivery.Orders.Api.Models;
using MongoDB.Driver;

namespace MicroDelivery.Orders.Api.Data
{
    public interface IOrdersRepository
    {
        Task<IEnumerable<Order>> GetOrdersAsync();
        Task<Order> GetOrderAsync(string id);
        Task<long> CountAllOrderAsync();

        Task<Order> CreateOrderAsync(Order order);
        Task<Order> UpdateOrderAsync(Order order);
        Task<bool> DeleteOrderAsync(string id);
    }

    public class OrdersRepository : IOrdersRepository
    {
        private readonly IMongoCollection<Order> orders;

        public OrdersRepository(IConfiguration configuration)
        {
            var clientSettings = MongoClientSettings.FromConnectionString(configuration.GetConnectionString("MongoDb"));
            var client = new MongoClient(clientSettings);
            var database = client.GetDatabase(configuration.GetValue<string>("MongoDb:DatabaseName"));

            this.orders = database.GetCollection<Order>(configuration.GetValue<string>("MongoDb:OrdersCollectionName"));
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            return await this.orders.Find(p => true).ToListAsync();
        }

        public async Task<Order> GetOrderAsync(string id)
        {
            return await this.orders.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<long> CountAllOrderAsync()
        {
            return await orders.EstimatedDocumentCountAsync();
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            await this.orders.InsertOneAsync(order);
            return order;
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            var result = await this.orders.ReplaceOneAsync(p => p.Id == order.Id, order);
            return order;
        }

        public async Task<bool> DeleteOrderAsync(string id)
        {
            var result = await this.orders.DeleteOneAsync(p => p.Id == id);
            return result.IsAcknowledged && result.DeletedCount == 1;
        }
    }
}
