using MongoDB.Bson.Serialization.Attributes;

namespace MicroDelivery.Orders.Api.Models
{
    public class Order
    {
        [BsonId]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public int CustomerId { get; set; }
        public string CustomerFirstName { get; set; } = string.Empty;
        public string CustomerLastName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public int TotalDiscount { get; set; }
        public IEnumerable<OrderLineItem> OrderLineItems { get; set; } = new List<OrderLineItem>();
        public DateTime? ShippedAtUtc { get; set; }
    }

    public class OrderLineItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double DiscountedPrice { get; set; }
    }
}
