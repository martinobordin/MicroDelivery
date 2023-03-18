namespace MicroDelivery.Shared.IntegrationEvents
{
    public record OrderSubmittedEvent(Guid CustomerId, IEnumerable<OrderSubmittedEventLineItem> OrderLineItems);

    public record OrderSubmittedEventLineItem(Guid ProductId, int Quantity);
}
