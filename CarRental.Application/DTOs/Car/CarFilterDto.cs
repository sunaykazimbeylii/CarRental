namespace CarRental.Application.DTOs.Car
{
    public record CarFilterDto
(
    string? Model,
    long? BrandId,
    long? CategoryId,
    decimal? MinPrice,
    decimal? MaxPrice,
    bool? IsAvailable
);
}
