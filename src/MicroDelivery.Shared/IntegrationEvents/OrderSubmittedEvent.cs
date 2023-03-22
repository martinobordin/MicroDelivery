namespace MicroDelivery.Shared.IntegrationEvents
{
    public record OrderSubmittedEvent(Guid OrderId, int CustomerId, string CustomerFirstName, string CustomerLastName, IEnumerable<OrderSubmittedEventLineItem> OrderLineItems);

    public record OrderSubmittedEventLineItem(int ProductId, string ProductName, int Quantity, double Price, double DiscountedPrice);
}
