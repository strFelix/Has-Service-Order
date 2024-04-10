using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OsDsII.api.Data;
using OsDsII.api.Models;

// interface segregation principal (SOL ->I D)
namespace OsDsII.api.Repository.ServiceOrders
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

        public async Task<ServiceOrder> GetCommentByServiceOrderIdAsync(int serviceOrderId)
        {
            ServiceOrder serviceOrderWithComments = await _dataContext.ServiceOrders
                .Include(c => c.Customer)
                .Include(c => c.Comments)
                .FirstOrDefaultAsync(s => s.Id == serviceOrderId);
            return (serviceOrderWithComments);
        }

        public async Task<ServiceOrder> GetServiceOrderCustomerAsync(int serviceOrderId)
        {
            ServiceOrder os = await _dataContext.ServiceOrders.Include(c => c.Customer).FirstOrDefaultAsync(s => serviceOrderId == s.Id);
            return (os);
        }
    }
}
