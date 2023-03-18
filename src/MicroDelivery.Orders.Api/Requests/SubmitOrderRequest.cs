namespace MicroDelivery.Orders.Api.Requests
{
    public record SubmitOrderRequest(Guid CustomerId, IEnumerable<SubmitOrderLineItem> OrderLineItems );

    public record SubmitOrderLineItem(Guid ProductId, int Quantity);
}
