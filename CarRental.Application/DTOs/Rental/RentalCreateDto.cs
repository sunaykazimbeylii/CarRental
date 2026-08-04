using CarRental.Domain.Enums;

namespace CarRental.Application.DTOs.Rental
{
    public record RentalCreateDto(
     long UserId,
     long CarId,
     DateTime StartDate,
     DateTime EndDate,
     PaymentMethod PaymentMethod
 );
}
