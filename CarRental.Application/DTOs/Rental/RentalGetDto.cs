using CarRental.Domain.Enums;

namespace CarRental.Application.DTOs.Rental
{
    public record RentalGetDto(
       long Id,
       string UserName,
       string CarModel,
       DateTime StartDate,
       DateTime EndDate,
       decimal TotalPrice,
       RentalStatus Status
   );
}
