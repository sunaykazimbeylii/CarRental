using CarRental.Application.DTOs.Car;

namespace CarRental.Application.Interfaces.Services
{
    public interface ICarService
    {
        Task CreateAsync(CarCreateDto dto);

        Task UpdateAsync(CarUpdateDto dto);

        Task DeleteAsync(long id);

        Task<CarGetDto> GetByIdAsync(long id);

        Task<IEnumerable<CarGetDto>> GetAllAsync();
    }
}
