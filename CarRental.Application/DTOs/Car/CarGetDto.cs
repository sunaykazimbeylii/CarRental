using CarRental.Domain.Enums;

namespace CarRental.Application.DTOs.Car
{
    public record CarGetDto(
  long Id,string Model,int Year,string PlateNumber,
  decimal DailyPrice,int Mileage, FuelType FuelType,
  Transmission Transmission,bool IsAvailable,string BrandName,
  string CategoryName, string BranchName, string ColorName
        );


   
}
