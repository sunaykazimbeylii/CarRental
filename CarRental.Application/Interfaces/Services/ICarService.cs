using CarRental.Application.DTOs.Car;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Application.Interfaces.Services
{
    public interface ICarService
    {
        Task CreateAsync(CarCreateDto dto);

        Task UpdateAsync(CarUpdateDto dto);

        Task DeleteAsync(long id);

        Task<CarGetDto> GetByIdAsync(long id);
     
        Task<IReadOnlyList<CarGetDto>> GetAllAsync(
    [FromQuery] CarFilterDto filter,
    int page,
    int take,
    string? sort
       );
    }
}
