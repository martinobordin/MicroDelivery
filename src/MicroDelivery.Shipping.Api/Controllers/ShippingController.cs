using Dapr;
using Dapr.Client;
using Grpc.Core;
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
        [Topic(DaprConstants.PubSubComponentName, DaprConstants.OrderSubmittedEventTopic)]
        public async Task<ActionResult> OnOrderSubmittedEvent(OrderSubmittedEvent orderSubmittedEvent)
        {
            await this.daprClient.InvokeBindingAsync(DaprConstants.HttpBinding, "post", orderSubmittedEvent);

            return Ok();
        }
    }
}