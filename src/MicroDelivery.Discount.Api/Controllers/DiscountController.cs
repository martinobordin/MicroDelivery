using Dapr.Client;
using MicroDelivery.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MicroDelivery.Discount.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly ILogger<DiscountController> logger;
        private readonly DaprClient daprClient;

        public DiscountController(ILogger<DiscountController> logger, DaprClient daprClient)
        {
            this.logger = logger;
            this.daprClient = daprClient;
        }

        [HttpPost("DiscountCronBinding")]
        public async Task<ActionResult> UpdateDiscountAsync()
        {
            var random = new Random(Guid.NewGuid().GetHashCode());
            var discount = random.Next(1, 30);

            logger.LogInformation($"Received UpdateDiscount {DateTime.Now} - Discount is now {discount}%");
            
            await daprClient.SaveStateAsync(DaprConstants.RedisStateComponentName, "Discount", discount);
            return Ok();
        }

        [HttpGet(Name = "GetDiscount")]
        public async Task<int> GetDiscount()
        {
            var discount = await daprClient.GetStateAsync<int>(DaprConstants.RedisStateComponentName, "Discount");
            var decimalDiscount = discount;
            return decimalDiscount;
        }
    }
}