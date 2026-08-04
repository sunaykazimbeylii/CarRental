using AutoMapper;
using CarRental.Application.DTOs.Car;
using CarRental.Application.Exceptions;
using CarRental.Application.Interfaces.Repositories;
using CarRental.Application.Interfaces.Services;
using CarRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CarRental.Application.Services;

public class CarService : ICarService
{
    private readonly ICarRepository _repository;
    private readonly IMapper _mapper;

    public CarService(
        ICarRepository repository,
        IMapper mapper)
    {
        _repository = repository;

        _mapper = mapper;

    }
    public async Task<CarGetDto> GetByIdAsync(long id)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity == null)
            throw new NotFoundException(nameof(Car));

        return _mapper.Map<CarGetDto>(entity);
    }


    public async Task<IReadOnlyList<CarGetDto>> GetAllAsync(
       CarFilterDto filter,
    int page,
    int take,
    string? sort)
    {
        Expression<Func<Car, object>>? sortExpression = sort?.ToLower() switch
        {
            "model" => x => x.Model,
            "price" => x => x.DailyPrice,
            "year" => x => x.Year,
            _ => x => x.Id
        };

        var query = _repository.GetAll(
    sort: sortExpression,
    page: page,
    take: take,
    includes: new[]
    {
        nameof(Car.Brand),
        nameof(Car.Category),
        nameof(Car.Branch),
        nameof(Car.Color)
    });
        if (!string.IsNullOrWhiteSpace(filter.Model))
            query = query.Where(x => x.Model.Contains(filter.Model));

        if (filter.BrandId.HasValue)
            query = query.Where(x => x.BrandId == filter.BrandId.Value);

        if (filter.CategoryId.HasValue)
            query = query.Where(x => x.CategoryId == filter.CategoryId.Value);

        if (filter.MinPrice.HasValue)
            query = query.Where(x => x.DailyPrice >= filter.MinPrice);

        if (filter.MaxPrice.HasValue)
            query = query.Where(x => x.DailyPrice <= filter.MaxPrice.Value);

        if (filter.IsAvailable.HasValue)
            query = query.Where(x => x.IsAvailable == filter.IsAvailable.Value);

        var cars = await query.ToListAsync();
        return _mapper.Map<IReadOnlyList<CarGetDto>>(cars);
    }


    public async Task CreateAsync(CarCreateDto dto)
    {
        var entity = _mapper.Map<Car>(dto);

        _repository.Add(entity);

        await _repository.SaveChangesAsync();
    }

    public async Task UpdateAsync(CarUpdateDto dto)
    {
        var entity = await _repository.GetByIdAsync(dto.Id);

        if (entity == null)
            throw new NotFoundException(nameof(Car));

        _mapper.Map(dto, entity);

        _repository.Update(entity);

        await _repository.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity == null)
            throw new NotFoundException(nameof(Car));

        _repository.Delete(entity);

        await _repository.SaveChangesAsync();
    }
}