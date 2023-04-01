using Dapr;
using Dapr.Client;
using MicroDelivery.Orders.Api.Data;
using MicroDelivery.Orders.Api.Dtos;
using MicroDelivery.Orders.Api.Models;
using MicroDelivery.Orders.Api.Requests;
using MicroDelivery.Shared;
using MicroDelivery.Shared.IntegrationEvents;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MicroDelivery.Orders.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private const string StreamName = "Orders";
        private readonly ILogger<OrdersController> logger;
        private readonly DaprClient daprClient;
        private readonly IOrdersRepository orderRepository;

        public OrdersController(ILogger<OrdersController> logger, DaprClient daprClient, IOrdersRepository orderRepository)
        {
            this.logger = logger;
            this.daprClient = daprClient;
            this.orderRepository = orderRepository;
        }

        [HttpPost("Submit")]
        [ProducesResponseType(typeof(Order), (int)HttpStatusCode.Created)]
        public async Task<ActionResult> SubmitOrder(SubmitOrderRequest request)
        {
            this.logger.LogInformation("SubmitOrder called");

            var customerInfo = await daprClient.InvokeMethodAsync<CustomerInfo>(HttpMethod.Get, DaprConstants.AppIdCustomers, $"customers/{request.CustomerId}");
            if (customerInfo is null)
            {
                throw new Exception($"Customer {request.CustomerId} not found");
            }

            var discount = await daprClient.InvokeMethodAsync<int>(HttpMethod.Get, DaprConstants.AppIdDiscount, "discount");

            var order = new Order
            {
                CustomerId = customerInfo.Id,
                CustomerFirstName = customerInfo.FirstName,
                CustomerLastName = customerInfo.LastName,
                CustomerEmail = customerInfo.Email,
                TotalDiscount = discount
            };

            var orderLineItems = new List<OrderLineItem>();
            foreach (var requestOrderLineItem in request.OrderLineItems)
            {
                var productInfo = await daprClient.InvokeMethodAsync<ProductInfo>(HttpMethod.Get, DaprConstants.AppIdProducts, $"products/{requestOrderLineItem.ProductId}");
                if (productInfo is null)
                {
                    throw new Exception($"Product {requestOrderLineItem.ProductId} not found");
                }

                var discountedPrice = discount == 0 ? productInfo.Price : productInfo.Price * (discount / 100);

                var orderLineItem = new OrderLineItem
                {
                    ProductId = productInfo.Id,
                    ProductName = productInfo.Name,
                    Price = productInfo.Price,
                    DiscountedPrice = discountedPrice,
                    Quantity = requestOrderLineItem.Quantity
                };
                orderLineItems.Add(orderLineItem);
            }

            order.OrderLineItems = orderLineItems;
            await orderRepository.CreateOrderAsync(order);

            this.logger.LogInformation("Order created");

            var orderSubmittedIntegrationEvent = new OrderSubmittedIntegrationEvent
            {
                OrderId = order.Id,
                CustomerId = order.CustomerId,
                CustomerFirstName = order.CustomerFirstName,
                CustomerLastName = order.CustomerLastName,
                CustomerEmail = order.CustomerEmail,
                TotalDiscount = order.TotalDiscount,
                OrderLineItems = order.OrderLineItems.Select(oli => new OrderSubmittedIntegrationEventLineItem { ProductId = oli.ProductId, ProductName = oli.ProductName, Quantity = oli.Quantity, Price = oli.Price, DiscountedPrice = oli.DiscountedPrice })
            };

            await daprClient.PublishEventAsync(DaprConstants.RabbitMqPubSubComponentName, DaprConstants.OrderSubmittedEventTopic, orderSubmittedIntegrationEvent);

            this.logger.LogInformation("OrderSubmittedEvent published");

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        [HttpPost("Ship")]
        [Topic(DaprConstants.RabbitMqPubSubComponentName, DaprConstants.OrderShippedEventTopic)]
        public async Task<ActionResult> OnOrderShippedEvent(OrderShippedIntegrationEvent orderShippedIntegrationEvent)
        {
            this.logger.LogInformation("OnOrderShippedEvent called");

            var order = await this.orderRepository.GetOrderAsync(orderShippedIntegrationEvent.OrderId);
            if (order is null)
            {
                return NotFound();
            }

            order.ShippedAtUtc = orderShippedIntegrationEvent.ShippedAtUtc;
            await this.orderRepository.UpdateOrderAsync(order);

            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Order>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await this.orderRepository.GetOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Order), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrder(Guid id)
        {
            var order = await this.orderRepository.GetOrderAsync(id);
            if (order is null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Order), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Order>> DeleteProduct(Guid id)
        {
            await this.orderRepository.DeleteOrderAsync(id);
            return Ok();
        }
    }
}