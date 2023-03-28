namespace MicroDelivery.Shared.IntegrationEvents
{
    public record OrderShippedIntegrationEvent : BaseIntegrationEvent
    {
        public Guid OrderId { get; set; }
        public DateTime ShippedAtUtc { get; set; }
    }
}
