using OsDsII.api.Models;

namespace OsDsII.api.Dtos
{
    public record CustomerDto(
        string Name, 
        string Email, 
        string Phone, 
        List<ServiceOrderDto> ListServiceOrder);
}
