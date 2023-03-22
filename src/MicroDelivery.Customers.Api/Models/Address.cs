namespace MicroDelivery.Customers.Api.Models
{
    public record Address
    {
        public string AddressLine { get; set; }
        public string ZipCode { get; set; }
        public string State { get; set; }
    }
}