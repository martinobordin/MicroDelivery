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
        [Topic(DaprConstants.PubSub, DaprConstants.OrderSubmittedEventTopic)]
        public ActionResult OnOrderSubmittedEvent(OrderSubmittedEvent @event)
        {
            this.logger.LogInformation("Sending email notification for order {order}", @event);
            return Ok();
        }
    }
}