using Dapr.Client;
using MicroDelivery.Orders.Api.Requests;
using MicroDelivery.Shared;
using MicroDelivery.Shared.IntegrationEvents;
using Microsoft.AspNetCore.Mvc;

namespace MicroDelivery.Orders.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> logger;
        private readonly DaprClient daprClient;

        public OrdersController(ILogger<OrdersController> logger, DaprClient daprClient)
        {
            this.logger = logger;
            this.daprClient = daprClient;
        }

        [HttpPost]
        public async Task<ActionResult> SubmitOrderAsync(SubmitOrderRequest request)
        {
            this.logger.LogInformation("SubmitOrder called");

            var @event = new OrderSubmittedEvent(request.CustomerId, request.OrderLineItems.Select(x => new OrderSubmittedEventLineItem(x.ProductId,x.Quantity)));
            await daprClient.PublishEventAsync(DaprConstants.PubSub, DaprConstants.OrderSubmittedEventTopic, @event);

            return Accepted();
        }
    }
}