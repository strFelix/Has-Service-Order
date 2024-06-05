using AutoMapper;
using Moq;
using OsDsII.api.Dtos.Customers;
using OsDsII.api.Models;
using OsDsII.api.Repository.CustomersRepository;
using OsDsII.api.Services.Customers;

namespace CalculadoraSalario.Tests
{
    public class CustomersServiceTests
    {
        private readonly Mock<ICustomersRepository> _mockCustomersRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CustomersService _service;

        public CustomersServiceTests()
        {
            _mockCustomersRepository = new Mock<ICustomersRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new CustomersService(_mockCustomersRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Should_Return_A_List_Of_Customers()
        {
            List<Customer> customers = new List<Customer>()
            {
                new Customer(1, "Lucas", "lucas@email.com", "912312312"),
                new Customer(2, "Redes", "redes@email.com", "932132132"),
                new Customer(3, "Brenda", "brenda@email.com", "943243243"),
            };

            List<CustomerDto> customersDto = new List<CustomerDto>()
            {
                new CustomerDto("Lucas", "lucas@email.com", "912312312", null),
                new CustomerDto("Redes", "redes@email.com", "932132132", null),
                new CustomerDto("Brenda", "brenda@email.com", "943243243", null),
            };

            _mockCustomersRepository.Setup(repository => repository.GetAllAsync()).ReturnsAsync(customers);
            var result = await _service.GetAllAsync();
            Assert.Equal(customersDto, result);
        }
    }
}