using OsDsII.api.Data;
using OsDsII.api.Models;

namespace OsDsII.api.Repository.Customers
{
    public interface ICustomerRepository
    {
        public Task<Customer> GetCustomerByServiceOrderAsync(ServiceOrder serviceOrder);
        public Task<List<Customer>> GetCustomersAsync();
        public Task<Customer> GetCustomerByIdAsync(int id);
        public Task<Customer> GetCustomerByEmailAsync(string email);
        public Task AddCustomerAsync(Customer customer);
        public Task DeleteCustomerAsync(Customer customer);
        public Task UpdateCustomerAsync(Customer customer);
    }
}
