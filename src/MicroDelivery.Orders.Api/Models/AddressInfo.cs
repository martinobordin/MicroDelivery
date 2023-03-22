namespace MicroDelivery.Orders.Api.Models
{
    public record AddressInfo
    {
        public string AddressLine { get; set; }
        public string ZipCode { get; set; }
        public string State { get; set; }
    }
}
