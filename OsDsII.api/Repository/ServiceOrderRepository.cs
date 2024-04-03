using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OsDsII.api.Data;
using OsDsII.api.Models;

// interface segregation principal (SOL ->I D)
namespace OsDsII.api.Repository
{
    public sealed class ServiceOrderRepository : IServiceOrderRepository
    {
        //repository layer
        // di data context

        private readonly DataContext _dataContext;
        public ServiceOrderRepository(DataContext dataContext) 
        { 
            _dataContext = dataContext;
        }
    
        public async Task<List<ServiceOrder>> GetAllAsync()
        {
            return await _dataContext.ServiceOrders.ToListAsync();
        }

        public async Task<ServiceOrder> GetByIdAsync(int id)
        {
            return await _dataContext.ServiceOrders.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddAsync(ServiceOrder serviceOrder)
        {
            await _dataContext.ServiceOrders.AddAsync(serviceOrder);
            await _dataContext.SaveChangesAsync();
        }

        public async Task FinishAsync(ServiceOrder serviceOrder)
        {
            _dataContext.ServiceOrders.Update(serviceOrder);
            await _dataContext.SaveChangesAsync();
        }

        public async Task CancelAsync(ServiceOrder serviceOrder)
        {
            _dataContext.ServiceOrders.Update(serviceOrder);
            await _dataContext.SaveChangesAsync();
        }
    }
}
