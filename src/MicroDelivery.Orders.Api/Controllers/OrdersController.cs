using Dapr.Client;
using MicroDelivery.Orders.Api.Models;
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

            var customerInfo = await daprClient.InvokeMethodAsync<CustomerInfo>(HttpMethod.Get, DaprConstants.AppIdCustomers, $"customers/{request.CustomerId}");

            var orderSubmittedEventLineItems = new List<OrderSubmittedEventLineItem>();
            foreach (var orderLineItem in request.OrderLineItems)
            {
                var productInfo = await daprClient.InvokeMethodAsync<ProductInfo>(HttpMethod.Get, DaprConstants.AppIdProducts, $"products/{orderLineItem.ProductId}");
                var discountedPrice = customerInfo.IsPremium ? productInfo.Price * 0.5 : productInfo.Price;
               
                orderSubmittedEventLineItems.Add(new OrderSubmittedEventLineItem(productInfo.Id, productInfo.Name, orderLineItem.Quantity, productInfo.Price, discountedPrice));
            }

            var @event = new OrderSubmittedEvent(Guid.NewGuid(),customerInfo.Id, customerInfo.FirstName, customerInfo.LastName, orderSubmittedEventLineItems);
            await daprClient.PublishEventAsync(DaprConstants.PubSubComponentName, DaprConstants.OrderSubmittedEventTopic, @event);

            return Accepted();
        }
    }
}