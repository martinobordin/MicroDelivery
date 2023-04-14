namespace MicroDelivery.Shared.IntegrationEvents
{
    public record OrderSubmittedIntegrationEvent : BaseIntegrationEvent
    {
        public string OrderId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerEmail { get; set; }
        public IEnumerable<OrderSubmittedIntegrationEventLineItem> OrderLineItems { get; set; }
        public int TotalDiscount { get; set; }
    }
    public record OrderSubmittedIntegrationEventLineItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double DiscountedPrice { get; set; }
    };
}
