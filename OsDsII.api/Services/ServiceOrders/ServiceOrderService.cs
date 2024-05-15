using AutoMapper;
using OsDsII.api.Dtos;
using OsDsII.api.Exceptions;
using OsDsII.api.Models;
using OsDsII.api.Repository.CustomersRepository;
using OsDsII.api.Repository.ServiceOrderRepository;

namespace OsDsII.api.Services.ServiceOrders
{
    public sealed class ServiceOrderService : IServiceOrderService
    {
        private readonly IServiceOrderRepository _serviceOrderRepository;
        private readonly ICustomersRepository _customersRepository;
        private readonly IMapper _mapper;

        public ServiceOrderService(IServiceOrderRepository serviceOrderRepository,ICustomersRepository customersRepository,IMapper mapper)
        {
            _serviceOrderRepository = serviceOrderRepository;
            _customersRepository = customersRepository;
            _mapper = mapper;
        }

        public async Task<List<ServiceOrderDto>> GetAllAsync()
        {
            List<ServiceOrder> serviceOrders = await _serviceOrderRepository.GetAllAsync();
            var serviceOrderDto = _mapper.Map<List<ServiceOrderDto>>(serviceOrders);
            return serviceOrderDto;
        }

        public async Task<ServiceOrderDto> GetServiceOrderAsync(int id)
        {
            ServiceOrder serviceOrder = await _serviceOrderRepository.GetByIdAsync(id);
            if (serviceOrder is null)
            {
                throw new NotFoundException("Service Order not found");
            }
            var serviceOrderDto = _mapper.Map<ServiceOrderDto>(serviceOrder);
            return serviceOrderDto;
        }

        public async Task CreateAsync(CreateServiceOrderDto createServiceOrderDto)
        {
            //NewServiceOrderDto
            if (createServiceOrderDto is null)
            {
                throw new Exception("Service order cannot be null");
            }

            Customer customer = await _customersRepository.GetByIdAsync(createServiceOrderDto.CustomerId);

            if (customer is null)
            {   
                throw new BadRequest("Service order cannot be linked to an unknown customer");
            }

            ServiceOrder serviceOrder = _mapper.Map<ServiceOrder>(createServiceOrderDto);
            await _serviceOrderRepository.AddAsync(serviceOrder);
            
            //return _mapper.Map<NewServiceOrderDto>(serviceOrder) 
        }
    }
}
