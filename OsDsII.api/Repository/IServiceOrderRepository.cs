using OsDsII.api.Models;

namespace OsDsII.api.Repository
{
    // dependency inversion principle (SOLI -> D)
    public interface IServiceOrderRepository
    {
        public Task<List<ServiceOrder>> GetAllAsync();
        public Task<ServiceOrder> GetByIdAsync(int id);
        public Task AddAsync(ServiceOrder serviceOrder);
        public Task FinishAsync(ServiceOrder serviceOrder);
        public Task CancelAsync(ServiceOrder serviceOrder);
    }
}
