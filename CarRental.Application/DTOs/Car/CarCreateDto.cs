using CarRental.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.Application.DTOs.Car
{
    public record CarCreateDto(string Model, int Year, string PlateNumber,
  decimal DailyPrice, int Mileage, FuelType FuelType,
  Transmission Transmission, bool IsAvailable, long BrandId,
    long CategoryId, long BranchId, long ColorId);


   }
}
