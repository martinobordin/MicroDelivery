namespace MicroDelivery.Shared.IntegrationEvents
{
    public abstract record BaseIntegrationEvent
    {
        public Guid EventId { get; } = Guid.NewGuid();
    }
}
