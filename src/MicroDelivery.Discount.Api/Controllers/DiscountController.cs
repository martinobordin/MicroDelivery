using Dapr.Client;
using MicroDelivery.Shared;
using Microsoft.AspNetCore.Mvc;

namespace MicroDelivery.Discount.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly ILogger<DiscountController> logger;
        private readonly DaprClient daprClient;
        private readonly IConfiguration configuration;

        public DiscountController(ILogger<DiscountController> logger, DaprClient daprClient, IConfiguration configuration)
        {
            this.logger = logger;
            this.daprClient = daprClient;
            this.configuration = configuration;
        }

        [HttpPost("/discountcronbinding")]
        public async Task<ActionResult> UpdateDiscount()
        {
            logger.LogInformation($"Received UpdateDiscount at {DateTime.Now}");

            var crazyDiscountEnabled = configuration.GetValue<bool>(DiscountConstants.CrazyDiscountEnabledKey);
            if (!crazyDiscountEnabled)
            {
                logger.LogInformation($"CrazyDiscount is disabled");
                return Ok();
            }

            var random = new Random(Guid.NewGuid().GetHashCode());
            var discount = random.Next(1, 30);

            logger.LogInformation($"CrazyDiscount is now {discount}%");
            
            await daprClient.SaveStateAsync(DaprConstants.RedisStateComponentName, DiscountConstants.CrazyDiscountValue, discount);
            return Ok();
        }

        [HttpGet]
        public async Task<int> GetDiscount()
        {
            var discount = 0;

            var crazyDiscountEnabled = configuration.GetValue<bool>(DiscountConstants.CrazyDiscountEnabledKey);
            if (crazyDiscountEnabled)
            {
                discount = await daprClient.GetStateAsync<int>(DaprConstants.RedisStateComponentName, DiscountConstants.CrazyDiscountValue);
            }
            
            return discount;
        }
    }
}