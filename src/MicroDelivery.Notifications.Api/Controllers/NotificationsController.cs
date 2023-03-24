using Dapr;
using Dapr.Client;
using MicroDelivery.Shared;
using MicroDelivery.Shared.IntegrationEvents;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace MicroDelivery.Notifications.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly ILogger<NotificationsController> logger;
        private readonly DaprClient daprClient;

        public NotificationsController(ILogger<NotificationsController> logger, DaprClient daprClient)
        {
            this.logger = logger;
            this.daprClient = daprClient;
        }

        [HttpPost]
        [Topic(DaprConstants.PubSubComponentName, DaprConstants.OrderSubmittedEventTopic)]
        public async Task<ActionResult> OnOrderSubmittedEventAsync(OrderSubmittedEvent orderSubmittedEvent)
        {
            this.logger.LogInformation("Sending notification for order #{OrderId} to {CustomerFirstName} {CustomerLastName} ({CustomerEmail})",
                orderSubmittedEvent.OrderId,
                orderSubmittedEvent.CustomerFirstName,
                orderSubmittedEvent.CustomerLastName,
                orderSubmittedEvent.CustomerEmail);

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Hello {orderSubmittedEvent.CustomerFirstName} {orderSubmittedEvent.CustomerLastName}<br>");
            stringBuilder.AppendLine($"Your order <strong>#{orderSubmittedEvent.OrderId.ToString()[..6]}</strong> has been shipped.<br><br>");
            stringBuilder.AppendLine($"Your CRAZY DISCOUNT is <strong>#{orderSubmittedEvent.TotalDiscount}%</strong>!<br><br>");
            stringBuilder.AppendLine($"Here your item(s):<br><ul>");

            foreach ( var item in orderSubmittedEvent.OrderLineItems )
            {
                stringBuilder.AppendLine($"<li>{item.Quantity} {item.ProductName} at <s>{item.Price.ToString("N2")}$</s> {item.DiscountedPrice.ToString("N2")}$</li>");
            }

            stringBuilder.AppendLine($"</ul><br>");
            stringBuilder.AppendLine($"Best regards!<br>");
            stringBuilder.AppendLine($"<strong>MicroDelivery Team</strong>");

            var message = stringBuilder.ToString();
            var metadata = new Dictionary<string, string>
            {
                { "emailTo", orderSubmittedEvent.CustomerEmail },
                { "subject", $"Order Shipped!" },
                { "priority", "1" }
            };
            await this.daprClient.InvokeBindingAsync(DaprConstants.SmtpBinding, "create", message, metadata);

            return Ok();
        }
    }
}