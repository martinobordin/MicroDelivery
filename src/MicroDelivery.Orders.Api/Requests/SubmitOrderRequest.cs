namespace MicroDelivery.Orders.Api.Requests
{
    public record SubmitOrderRequest(int CustomerId, IEnumerable<SubmitOrderLineItem> OrderLineItems );

    public record SubmitOrderLineItem(int ProductId, int Quantity);
}
