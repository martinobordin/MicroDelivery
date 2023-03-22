namespace MicroDelivery.Orders.Api.Models
{
    public class ProductInfo
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; } = 0;
    }
}
