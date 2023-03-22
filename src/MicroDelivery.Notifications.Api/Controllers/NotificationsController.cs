using Dapr;
using MicroDelivery.Shared;
using MicroDelivery.Shared.IntegrationEvents;
using Microsoft.AspNetCore.Mvc;

namespace MicroDelivery.Notifications.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly ILogger<NotificationsController> logger;

        public NotificationsController(ILogger<NotificationsController> logger)
        {
            this.logger = logger;
        }

        [HttpPost]
        [Topic(DaprConstants.PubSubComponentName, DaprConstants.OrderSubmittedEventTopic)]
        public ActionResult OnOrderSubmittedEvent(OrderSubmittedEvent orderSubmittedEvent)
        {
            this.logger.LogInformation("Sending notification for order #{OrderId} - {CustomerFirstName} {CustomerLastName}", orderSubmittedEvent.OrderId, orderSubmittedEvent.CustomerFirstName, orderSubmittedEvent.CustomerLastName);
            foreach (var orderLineItem in orderSubmittedEvent.OrderLineItems)
            {
                this.logger.LogDebug("Product: {ProductName} - Qty: {Quantity} - Price: {Price} - DiscountedPrice: {DiscountedPrice}", orderLineItem.ProductName, orderLineItem.Quantity, orderLineItem.Price, orderLineItem.DiscountedPrice);
            }
            return Ok();
        }
    }
}