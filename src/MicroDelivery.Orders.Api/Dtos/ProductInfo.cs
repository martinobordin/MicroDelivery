namespace MicroDelivery.Orders.Api.Dtos
{
    public class ProductInfo
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; } = 0;
    }
}
