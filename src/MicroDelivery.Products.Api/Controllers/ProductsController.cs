using Dapr.Client;
using MicroDelivery.Shared;
using Microsoft.AspNetCore.Mvc;

namespace MicroDelivery.Products.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> logger;
        private readonly DaprClient daprClient;

        public ProductsController(ILogger<ProductsController> logger, DaprClient daprClient)
        {
            this.logger = logger;
            this.daprClient = daprClient;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductDto>> GetAsync()
        {
            this.logger.LogInformation("Get products called");

            var cachedProducts = await daprClient.GetStateAsync<IEnumerable<ProductDto>>(DaprConstants.MongoStateStore, "products.get");
            if (cachedProducts == null)
            {
                cachedProducts = Enumerable
                    .Range(1, 5)
                    .Select(index => new ProductDto
                    {
                        Sku = $"Sku_{index}",
                        Description = $"Description_{index}",
                        LastUpdate = DateTime.Now
                    });

                this.logger.LogInformation("Saving products to state");

                await daprClient.SaveStateAsync(DaprConstants.MongoStateStore, "products.get", cachedProducts, metadata: new Dictionary<string, string>() { { "ttlInSeconds", "5" } });
            }

            this.logger.LogInformation("Returning products from state");

            return cachedProducts;
        }
    }
}