using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OsDsII.api.Data;
using OsDsII.api.Models;

namespace OsDsII.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServiceOrdersController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public ServiceOrdersController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllServiceOrderAsync()
        {
            try
            {
                List<ServiceOrder> serviceOrders = await _dataContext.ServiceOrders.ToListAsync();
                return Ok(serviceOrders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceOrderById(int id)
        {
            try
            {
                ServiceOrder serviceOrder = await _dataContext.ServiceOrders.FirstOrDefaultAsync(s => s.Id == id);
                if (serviceOrder is null)
                {
                    throw new Exception("Service order not found");
                }
                return Ok(serviceOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateServiceOrderAsync(ServiceOrder serviceOrder)
        {
            try
            {
                if (serviceOrder is null)
                {
                    throw new Exception("Service order cannot be null");
                }

                Customer customer = await _dataContext.Customers.FirstOrDefaultAsync(c => serviceOrder.Customer.Id == c.Id);

                if (customer is null)
                {
                    throw new Exception("Customer not found");
                }

                await _dataContext.ServiceOrders.AddAsync(serviceOrder);
                var affectedRow = await _dataContext.SaveChangesAsync();
                return Ok(affectedRow);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("{id}/status/finish")]
        public async Task<IActionResult> FinishServiceOrderAsync(int id)
        {
            try
            {
                ServiceOrder serviceOrder = await _dataContext.ServiceOrders.FirstOrDefaultAsync(s => s.Id == id);
                if (serviceOrder is null)
                {
                    throw new Exception("Service order cannot be null");
                }

                serviceOrder.FinishOS();
                _dataContext.ServiceOrders.Update(serviceOrder);
                await _dataContext.SaveChangesAsync();
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/status/cancel")]
        public async Task<IActionResult> CancelServiceOrder(int id)
        {
            try
            {
                ServiceOrder serviceOrder = await _dataContext.ServiceOrders.FirstOrDefaultAsync(s => s.Id == id);
                if (serviceOrder is null)
                {
                    throw new Exception("Service order cannot be null");
                }

                serviceOrder.Cancel();
                _dataContext.ServiceOrders.Update(serviceOrder);
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