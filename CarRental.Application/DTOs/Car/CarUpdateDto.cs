using CarRental.Domain.Enums;

namespace CarRental.Application.DTOs.Car
{
    public record CarUpdateDto(long Id, string Model, int Year, string PlateNumber,
  decimal DailyPrice, int Mileage, FuelType FuelType,
  Transmission Transmission, bool IsAvailable, long BrandId,
    long CategoryId, long BranchId, long ColorId);


}
