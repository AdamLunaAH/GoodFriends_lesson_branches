using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

using Services;
using Configuration;
using Configuration.Options;

using Microsoft.Extensions.Options;
using Models;

namespace AppWebApi
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CustomerController : Controller
    {
        readonly ICustomerService _service;
        private readonly IEncryptionService _encryptionService;
        readonly ILogger<CustomerController> _logger;

        [HttpGet()]
        [ActionName("Clear")]
        [ProducesResponseType(200, Type = typeof(List<ICustomer>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public IActionResult Clear(int nrItems)
        {
            try
            {

                _logger.LogInformation($"{nameof(Clear)} called.");
                var customers = _service.GetCustomers(nrItems);
                return Ok(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(Clear)}: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet()]
        [ActionName("Encrypted")]
        [ProducesResponseType(200, Type = typeof(List<ICustomer>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public IActionResult Encrypted(int nrItems)
        {
            try
            {
                _logger.LogInformation($"{nameof(Encrypted)} called.");

                var customers = _service.GetCustomers(nrItems);


                // Encrypt customer cards
                foreach (var customer in customers)
                {
                    if (customer.CreditCard != null)
                    {
                        customer.EncryptedCardData = _encryptionService.EncryptCardData(customer.CreditCard);
                        // Removes card data after encryption
                        // customer.CreditCard = null;
                    }
                }

                return Ok(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(Encrypted)}: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        public CustomerController(ICustomerService service, IEncryptionService encryptionService, ILogger<CustomerController> logger)
        {
            _service = service;
            _encryptionService = encryptionService;
            _logger = logger;
        }
    }
}