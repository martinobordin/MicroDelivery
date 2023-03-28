using Dapr.Client;
using MicroDelivery.Customers.Api.Data;
using MicroDelivery.Customers.Api.Models;
using MicroDelivery.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MicroDelivery.Customers.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ILogger<CustomersController> logger;
        private readonly DaprClient daprClient;
        private readonly ICustomerRepository customerRepository;

        private const string StateKey = "GetCustomers";
        private readonly Dictionary<string, string> stateMetaData = new() { { "ttlInSeconds", "10" } };

        public CustomersController(ILogger<CustomersController> logger, DaprClient daprClient, ICustomerRepository customerRepository)
        {
            this.logger = logger;
            this.daprClient = daprClient;
            this.customerRepository = customerRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Customer>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            var customers = await GetCustomersFromCache();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Customer), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomer(int id)
        {
            var customers = await GetCustomersFromCache();
            var customer = customers?.FirstOrDefault(x => x.Id == id);
            if (customer is null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Customer), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Customer>> CreateCustomer(Customer product)
        {
            await this.customerRepository.CreateCustomerAsync(product);
            await ClearCustomersCache();
            return CreatedAtAction(nameof(GetCustomer), new { id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Customer), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Customer>> UpdateProduct(Customer product)
        {
            await this.customerRepository.UpdateCustomerAsync(product);
            await ClearCustomersCache();
            return Ok(product);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Customer), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Customer>> DeleteProduct(int id)
        {
            await this.customerRepository.DeleteCustomerAsync(id);
            await ClearCustomersCache();
            return Ok();
        }

        private async Task<IEnumerable<Customer>> GetCustomersFromCache()
        {
            logger.LogInformation($"{nameof(GetCustomersFromCache)} called. Checking the state");

            var customers = await daprClient.GetStateAsync<IEnumerable<Customer>>(DaprConstants.MongoStateComponentName, StateKey);
            if (customers == null)
            {
                customers = await this.customerRepository.GetCustomersAsync();
                await daprClient.SaveStateAsync(DaprConstants.MongoStateComponentName, StateKey, customers, metadata: stateMetaData);

                logger.LogInformation($"{nameof(GetCustomersFromCache)} called. State updated");
            }

            return customers;
        }

        private async Task ClearCustomersCache()
        {
            await daprClient.DeleteStateAsync(DaprConstants.MongoStateComponentName, StateKey);
        }
    }
}