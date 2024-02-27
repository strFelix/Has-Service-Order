using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OsDsII.api.Data;
using OsDsII.api.Models;

namespace OsDsII.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public CustomersController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            List<Customer> customers = await _dataContext.Customers.ToListAsync();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                Customer customer = await _dataContext.Customers.FirstOrDefaultAsync(c => c.Id == id);
                if (customer is null)
                {
                    return BadRequest("Customer not found");
                }
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(Customer customer)
        {
            try
            {
                Customer customerExists = await _dataContext.Customers.FirstOrDefaultAsync(c => c.Email == customer.Email);
                if (customerExists != null && !customerExists.Equals(customer))
                {
                    return BadRequest("Customer already exists");
                }
                await _dataContext.Customers.AddAsync(customer);
                await _dataContext.SaveChangesAsync();

                return Ok(customer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerAsync(int id)
        {
            try
            {
                Customer customer = await _dataContext.Customers.FirstOrDefaultAsync(c => c.Id == id);
                _dataContext.Customers.Remove(customer);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomerAsync(Customer customer)
        {
            try
            {
                Customer currentCustomer = await _dataContext.Customers.FirstOrDefaultAsync(c => c.Id == customer.Id);
                if (customer is null)
                {
                    throw new Exception("Customer not found");
                }
                _dataContext.Customers.Update(customer);
                await _dataContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}