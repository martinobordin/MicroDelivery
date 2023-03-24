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
            var discount = await daprClient.InvokeMethodAsync<int>(HttpMethod.Get, DaprConstants.AppIdDiscount, "discount");

            var orderSubmittedEventLineItems = new List<OrderSubmittedEventLineItem>();
            foreach (var orderLineItem in request.OrderLineItems)
            {
                var productInfo = await daprClient.InvokeMethodAsync<ProductInfo>(HttpMethod.Get, DaprConstants.AppIdProducts, $"products/{orderLineItem.ProductId}");
                var discountedPrice = discount == 0 ? productInfo.Price : productInfo.Price * (discount / 100);

                orderSubmittedEventLineItems.Add(new OrderSubmittedEventLineItem { ProductId = productInfo.Id, ProductName = productInfo.Name, Quantity = orderLineItem.Quantity, Price = productInfo.Price, DiscountedPrice = discountedPrice });
            }

            var @event = new OrderSubmittedEvent { OrderId = Guid.NewGuid(), CustomerId = customerInfo.Id, CustomerFirstName = customerInfo.FirstName, CustomerLastName = customerInfo.LastName, CustomerEmail = customerInfo.Email, TotalDiscount = discount, OrderLineItems = orderSubmittedEventLineItems };
            await daprClient.PublishEventAsync(DaprConstants.PubSubComponentName, DaprConstants.OrderSubmittedEventTopic, @event);

            return Accepted();
        }
    }
}