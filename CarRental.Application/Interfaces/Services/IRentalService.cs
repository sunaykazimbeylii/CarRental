using CarRental.Application.DTOs.Rental;

namespace CarRental.Application.Interfaces.Services
{
    public interface IRentalService
    {
        Task CreateAsync(RentalCreateDto dto);
        Task<List<RentalGetDto>> GetAllAsync();
        Task<RentalGetDto> GetByIdAsync(long id);
    }
}
