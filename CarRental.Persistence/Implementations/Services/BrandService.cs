using AutoMapper;
using CarRental.Application.DTOs.Brand;
using CarRental.Application.Interfaces.Repositories;
using CarRental.Application.Interfaces.Services;
using CarRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace CarRental.Persistence.Implementations.Services
{
    internal class BrandService:IBrandService
    {
        private readonly IBrandRepository _repository;
        private readonly IMapper _mapper;

        public BrandService(IBrandRepository repository,IMapper mapper)
        {
           _repository = repository;
            _mapper = mapper;
        }

        public async Task CreateAsync(BrandCreateDto dto)
        {
            bool result = await _repository.AnyAsync(b=>b.Name== dto.Name);
            if (result)
            {
                throw new Exception($"Brand named'{dto.Name}' already exists");
            }
            Brand brand = _mapper.Map<Brand>(dto);
              _repository.Add(brand);
            brand.CreatedAt= DateTime.UtcNow;
            await _repository.SaveChangesAsync();

        }

        public async Task DeleteAsync(long id)
        {
            Brand existed= await _repository.GetByIdAsync(id);
            if (existed is null) throw new Exception($"Brand is not found");
            _repository.Delete(existed);
            await _repository.SaveChangesAsync();  
           
        }


public async Task<IReadOnlyList<BrandGetDto>> GetAllAsync(int page, int take, string? sort)
    {
        var brands =  _repository
            .GetAll(
                sort: c => c.Name,
                page: page,
                take: take,
                includes: nameof(Brand.Cars))
            .ToListAsync();

        return _mapper.Map<IReadOnlyList<BrandGetDto>>(brands);
    }

    public async Task<BrandGetDto> GetById(long id)
        {
            Brand? brand = await _repository.GetByIdAsync(id, nameof(Brand.Cars));
            if (brand is null) throw new Exception("brand is not found");
            return _mapper.Map<BrandGetDto>(brand);
        }
       

      

        public async Task UpdateAsync(long id,BrandUpdateDto dto)
        {
            bool result = await _repository.AnyAsync(b => b.Name == dto.Name && b.Id != id);

            if (result)
                throw new Exception($"Brand named'{dto.Name}' already exists");

            Brand existed = await _repository.GetByIdAsync(id);

            if (existed is null) throw new Exception("Brand is not found");

            _mapper.Map(dto, existed);

            existed.UpdatedAt = DateTime.UtcNow;

            _repository.Update(existed);

            await _repository.SaveChangesAsync();

        }
    }
}
