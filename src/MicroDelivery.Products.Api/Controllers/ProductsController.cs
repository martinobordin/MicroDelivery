using Dapr.Client;
using MicroDelivery.Products.Api.Data;
using MicroDelivery.Products.Api.Models;
using MicroDelivery.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MicroDelivery.Products.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> logger;
        private readonly DaprClient daprClient;
        private readonly IProductRepository productRepository;

        private const string StateKey = "GetProducts";
        private readonly Dictionary<string, string> stateMetaData = new() { { "ttlInSeconds", "10" } };

        public ProductsController(ILogger<ProductsController> logger, DaprClient daprClient, IProductRepository productRepository)
        {
            this.logger = logger;
            this.daprClient = daprClient;
            this.productRepository = productRepository; 
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            logger.LogInformation($"{nameof(GetProducts)} called. Checking the state");
            
            var products = await daprClient.GetStateAsync<IEnumerable<Product>>(DaprConstants.RedisStateStore, StateKey);
            if (products == null)
            {
                products = await this.productRepository.GetProductsAsync();
                await daprClient.SaveStateAsync(DaprConstants.RedisStateStore, StateKey, products, metadata: stateMetaData);

                logger.LogInformation($"{nameof(GetProducts)} called. State updated");
            }
                
            return Ok(products);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct(Guid id)
        {
            var product = await this.productRepository.GetProductAsync(id);
            if (product is null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            await this.productRepository.CreateProductAsync(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> UpdateProduct(Product product)
        {
            await this.productRepository.UpdateProductAsync(product);
            return Ok(product);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> DeleteProduct(Guid id)
        {
            await this.productRepository.DeleteProductAsync(id);
            return Ok();
        }
    }
}