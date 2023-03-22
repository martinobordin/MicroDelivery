using Dapr.Client;
using MicroDelivery.Shared;
using Microsoft.AspNetCore.Mvc;

namespace MicroDelivery.Customers.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {

        private readonly ILogger<CustomersController> logger;
        private readonly DaprClient daprClient;

        public CustomersController(ILogger<CustomersController> logger, DaprClient daprClient)
        {
            this.logger = logger;
            this.daprClient = daprClient;
        }

        [HttpGet()]
        public async Task<IEnumerable<CustomerDto>> Get()
        {
            this.logger.LogInformation("Get customers called");

            var cachedCustomers = await daprClient.GetStateAsync<IEnumerable<CustomerDto>>(DaprConstants.RedisStateStore, "customers.get");
            if (cachedCustomers == null) {
                cachedCustomers = Enumerable
                    .Range(1, 5)
                    .Select(index => new CustomerDto
                    {
                        FirstName = $"FirstName_{index}",
                        LastName = $"LastName_{index}",
                        LastUpdate = DateTime.Now
                    });

                this.logger.LogInformation("Saving customers to state");

                await daprClient.SaveStateAsync(DaprConstants.RedisStateStore, "customers.get", cachedCustomers, metadata: new Dictionary<string, string>() { { "ttlInSeconds", "5" } });
            }

            this.logger.LogInformation("Returning customers from state");

            return cachedCustomers;
        }
    }
}