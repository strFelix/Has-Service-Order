using OsDsII.api.Dtos;

namespace OsDsII.api.Services.ServiceOrders
{
    public interface IServiceOrderService
    {
        public Task<List<ServiceOrderDto>> GetAllAsync();
        public Task<ServiceOrderDto> GetServiceOrderAsync(int id);
        public Task<CreateServiceOrderDto> CreateServiceOrderAsync(CreateServiceOrderDto createServiceOrderDto);
        
    }
}
