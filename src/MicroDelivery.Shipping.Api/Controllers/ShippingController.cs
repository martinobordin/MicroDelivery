using Dapr;
using Dapr.Client;
using MicroDelivery.Shared;
using MicroDelivery.Shared.IntegrationEvents;
using Microsoft.AspNetCore.Mvc;

namespace MicroDelivery.Shipping.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShippingController : ControllerBase
    {
        private readonly ILogger<ShippingController> logger;
        private readonly DaprClient daprClient;

        public ShippingController(ILogger<ShippingController> logger, DaprClient daprClient)
        {
            this.logger = logger;
            this.daprClient = daprClient;
        }

        [HttpPost]
        [Topic(DaprConstants.RabbitMqPubSubComponentName, DaprConstants.OrderSubmittedEventTopic)]
        public async Task<ActionResult> OnOrderSubmittedEvent(OrderSubmittedIntegrationEvent orderSubmittedIntegrationEvent)
        {
            this.logger.LogInformation("Shipping order #{OrderId} to {CustomerFirstName} {CustomerLastName} ({CustomerEmail})",
             orderSubmittedIntegrationEvent.OrderId,
             orderSubmittedIntegrationEvent.CustomerFirstName,
             orderSubmittedIntegrationEvent.CustomerLastName,
             orderSubmittedIntegrationEvent.CustomerEmail);

            await this.daprClient.InvokeBindingAsync(DaprConstants.HttpBinding, "post", orderSubmittedIntegrationEvent);

            // Simulate shipping delay
            await Task.Delay(1000);

            var orderShippedIntegrationEvent = new OrderShippedIntegrationEvent() { OrderId = orderSubmittedIntegrationEvent.OrderId, ShippedAtUtc = DateTime.UtcNow };
            await daprClient.PublishEventAsync(DaprConstants.RabbitMqPubSubComponentName, DaprConstants.OrderShippedEventTopic, orderShippedIntegrationEvent);

            return Ok();
        }
    }
}