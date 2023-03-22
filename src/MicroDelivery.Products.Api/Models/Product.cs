namespace MicroDelivery.Products.Api.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public IEnumerable<string> Categories { get; set; } = Enumerable.Empty<string>();
        public decimal Price { get; set; } = 0;
        public DateTime LastUpdate { get; set; } = DateTime.Now;
    }
}
