using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OsDsII.api.Dtos;
using OsDsII.api.Exceptions;
using OsDsII.api.Models;
using OsDsII.api.Repository.CustomersRepository;
using OsDsII.api.Repository.ServiceOrderRepository;
using OsDsII.api.Services.Customers;
using OsDsII.api.Services.ServiceOrders;

namespace OsDsII.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public sealed class ServiceOrdersController : ControllerBase
    {
        private readonly IServiceOrderService _serviceOrderService;
        private readonly ICustomersService _customersService;
        private readonly IMapper _mapper;

        public ServiceOrdersController(ICustomersService customersService, IServiceOrderService serviceOrderService,IMapper mapper)
        {
            _serviceOrderService = serviceOrderService;
            _customersService = customersService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllServiceOrderAsync()
        {
            try
            {
                List<ServiceOrderDto> serviceOrders = await _serviceOrderService.GetAllAsync();
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
                ServiceOrderDto serviceOrder = await _serviceOrderService.GetServiceOrderAsync(id);
                return Ok(serviceOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateServiceOrderAsync(CreateServiceOrderDto serviceOrderDto)
        {
            try
            {
                //new service (service order puro)
                ServiceOrder serviceOrder = _serviceOrderService.CreateServiceOrderAsync(serviceOrderDto);

                CustomerDto customer = await _customersService.GetCustomerAsync(serviceOrderDto.CustomerId);

                
                return Created("CreateServiceOrderAsync", serviceOrder);
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
                ServiceOrder serviceOrder = await _serviceOrderRepository.GetByIdAsync(id);
                if (serviceOrder is null)
                {
                    throw new Exception("Service order cannot be null");
                }

                serviceOrder.FinishOS();
                await _serviceOrderRepository.FinishAsync(serviceOrder);
                return NoContent();

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
                ServiceOrder serviceOrder = await _serviceOrderRepository.GetByIdAsync(id);
                if (serviceOrder is null)
                {
                    throw new Exception("Service order cannot be null");
                }

                serviceOrder.Cancel();
                await _serviceOrderRepository.CancelAsync(serviceOrder);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}