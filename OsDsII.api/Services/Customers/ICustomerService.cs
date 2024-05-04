using OsDsII.api.Dtos;

namespace OsDsII.api.Services.Customers
{
    public interface ICustomersService
    {
        public Task<IEnumerable<CustomerDto>> GetAllAsync();
        public Task<CustomerDto> GetCustomerAsync(int id);
        public Task CreateAsync(CreateCustomerDto customer);
        public Task UpdateAsync(int id);
        public Task DeleteAsync(int id);
    }
}