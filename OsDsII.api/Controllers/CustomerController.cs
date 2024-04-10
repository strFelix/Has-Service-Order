using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OsDsII.api.Data;
using OsDsII.api.Models;
using OsDsII.api.Repository.Customers;

namespace OsDsII.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                //GetCustomersAsync
                List<Customer> customers = await _customerRepository.GetCustomersAsync();
                return Ok(customers);
            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                //GetCustomerByIdAsync
                Customer customer = await _customerRepository.GetCustomerByIdAsync(id);
                if (customer is null)
                {
                    return NotFound("Customer not found");
                }
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Customer))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCustomerAsync(Customer customer)
        {
            try
            {
                //GetCustomerByEmailAsync
                Customer customerExists = await _customerRepository.GetCustomerByEmailAsync(customer.Email);
                if (customerExists != null && !customerExists.Equals(customer))
                {
                    return Conflict("Customer already exists");
                }
                //AddCustomerAsync
                await _customerRepository.AddCustomerAsync(customer);

                return Created(nameof(CustomersController),customer);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCustomerAsync(int id)
        {
            try
            {
                //GetCustomerByIdAsync
                Customer customer = await _customerRepository.GetCustomerByIdAsync(id);
                if (customer is null)
                {
                    return NotFound("Customer not found");
                }
                //DeleteCustomerAsync
                await _customerRepository.DeleteCustomerAsync(customer);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCustomerAsync(Customer customer)
        {
            try
            {
                //GetCustomerByIdAsync
                Customer currentCustomer = await _customerRepository.GetCustomerByIdAsync(customer.Id);
                if (customer is null)
                {
                    return NotFound("Customer not found");
                }
                //UpdateCustomerAsync
                await _customerRepository.UpdateCustomerAsync(currentCustomer);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}