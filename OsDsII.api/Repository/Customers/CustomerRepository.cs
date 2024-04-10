using Microsoft.EntityFrameworkCore;
using OsDsII.api.Data;
using OsDsII.api.Models;

namespace OsDsII.api.Repository.Customers
{
    public sealed class CustomerRepository : ICustomerRepository
    {
        private readonly DataContext _dataContext;
        public CustomerRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Customer> GetCustomerByServiceOrderAsync(ServiceOrder serviceOrder)
        {
            Customer customer = await _dataContext.Customers.FirstOrDefaultAsync(c => serviceOrder.Customer.Id == c.Id);
            return (customer);
        }

        public async Task<List<Customer>> GetCustomersAsync()
        {
            List<Customer> customers = await _dataContext.Customers.ToListAsync();
            return(customers);
        }
        
        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            Customer customer = await _dataContext.Customers.FirstOrDefaultAsync(c => c.Id == id);
            return (customer);
        }

        public async Task<Customer> GetCustomerByEmailAsync(string email)
        {
            Customer customerExists = await _dataContext.Customers.FirstOrDefaultAsync(c => c.Email == email);
            return (customerExists);
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            await _dataContext.Customers.AddAsync(customer);
            await _dataContext.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(Customer customer)
        {
            _dataContext.Customers.Remove(customer);
            await _dataContext.SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            _dataContext.Customers.Update(customer);
            await _dataContext.SaveChangesAsync();
        }
    }
}
