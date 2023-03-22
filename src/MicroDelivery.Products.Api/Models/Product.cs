using MongoDB.Bson.Serialization.Attributes;

namespace MicroDelivery.Products.Api.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.Int32)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public IEnumerable<string> Categories { get; set; } = Enumerable.Empty<string>();
        public double Price { get; set; } = 0;
        public DateTime LastUpdate { get; set; } = DateTime.Now;
    }
}
