using CarRental.Application.DTOs.Brand;

namespace CarRental.Application.Interfaces.Services
{
    public interface IBrandService
    {
        Task<IReadOnlyList<BrandGetDto>> GetAllAsync(int page, int take, string? sort);
        Task<BrandGetDto> GetById(long  id);
        Task CreateAsync(BrandCreateDto dto);
        Task UpdateAsync(long id,BrandUpdateDto dto);
        Task DeleteAsync(long id);

    }
}
