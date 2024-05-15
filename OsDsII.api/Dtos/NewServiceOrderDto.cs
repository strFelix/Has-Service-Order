using OsDsII.api.Models;

namespace OsDsII.api.Dtos
{
    public record NewServiceOrderDto(
        int Id, 
        string Description, 
        double Price, 
        StatusServiceOrder Status, 
        DateTimeOffset 
        OpeningDate, 
        DateTimeOffset? FinishDate, 
        int CustomerId
        );
}