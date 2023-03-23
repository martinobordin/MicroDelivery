namespace MicroDelivery.Shared.IntegrationEvents
{
    public record OrderSubmittedEvent
    {
        public Guid OrderId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerEmail { get; set; }
        public IEnumerable<OrderSubmittedEventLineItem> OrderLineItems { get; set; }
    };

    public record OrderSubmittedEventLineItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double DiscountedPrice { get; set; }
    };
}
