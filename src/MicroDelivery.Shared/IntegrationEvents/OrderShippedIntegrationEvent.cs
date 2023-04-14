namespace MicroDelivery.Shared.IntegrationEvents
{
    public record OrderShippedIntegrationEvent : BaseIntegrationEvent
    {
        public string OrderId { get; set; }
        public DateTime ShippedAtUtc { get; set; }
    }
}
