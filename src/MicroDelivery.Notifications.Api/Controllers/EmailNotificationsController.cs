using Dapr;
using Dapr.Client;
using MicroDelivery.Shared;
using MicroDelivery.Shared.IntegrationEvents;
using Microsoft.AspNetCore.Mvc;

namespace MicroDelivery.Notifications.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmailNotificationsController : ControllerBase
    {
        private readonly ILogger<EmailNotificationsController> logger;

        public EmailNotificationsController(ILogger<EmailNotificationsController> logger)
        {
            this.logger = logger;
        }

        [HttpPost]
        [Topic(DaprConstants.PubSubComponentName, DaprConstants.OrderSubmittedEventTopic)]
        public ActionResult OnOrderSubmittedEvent(OrderSubmittedEvent orderSubmittedEvent)
        {
            this.logger.LogInformation("Sending email notification for order #{OrderId} - {CustomerFirstName} {CustomerLastName}", orderSubmittedEvent.OrderId, orderSubmittedEvent.CustomerFirstName, orderSubmittedEvent.CustomerLastName);
            foreach (var orderLineItem in orderSubmittedEvent.OrderLineItems)
            {
                this.logger.LogDebug("Product: {ProductName} - Qty: {Quantity} - Price: {Price} - DiscountedPrice: {DiscountedPrice}", orderLineItem.ProductName, orderLineItem.Quantity, orderLineItem.Price, orderLineItem.DiscountedPrice);
            }
            return Ok();
        }
    }
}