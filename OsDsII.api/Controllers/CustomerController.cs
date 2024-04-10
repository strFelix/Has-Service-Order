using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OsDsII.api.Data;
using OsDsII.api.Dtos;
using OsDsII.api.Models;
using OsDsII.api.Repository.CustomersRepository;

namespace OsDsII.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        //private readonly DataContext _dataContext;
        private readonly ICustomersRepository _customersRepository;

        public CustomersController(ICustomersRepository customersRepository)
        {
            _customersRepository = customersRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                IEnumerable<Customer> customers = await _customersRepository.GetAllAsync();
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                Customer customer = await _customersRepository.GetByIdAsync(id);
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
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Customer))]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCustomerAsync(CreateCustomerDto customer)
        {
            try
            {
                Customer customerExists = await _customersRepository.FindUserByEmailAsync(customer.Email);
                if (customerExists != null && !customerExists.Equals(customer))
                {
                    return Conflict("Customer already exists");
                }
                await _customersRepository.AddCustomerAsync(customerExists);

                return Created(nameof(CustomersController), customer);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
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
                Customer customer = await _customersRepository.GetByIdAsync(id);
                if (customer is null)
                {
                    return NotFound("Customer not found");
                }
                await _customersRepository.DeleteCustomer(customer);
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
                Customer currentCustomer = await _customersRepository.GetByIdAsync(customer.Id);
                if (customer is null)
                {
                    return NotFound("Customer not found");
                }
                await _customersRepository.UpdateCustomerAsync(customer);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}