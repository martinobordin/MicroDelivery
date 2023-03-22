namespace MicroDelivery.Orders.Api.Models
{
    public class CustomerInfo
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public AddressInfo Address { get; set; } = new AddressInfo();
        public bool IsPremium { get; set; }
    }
}
