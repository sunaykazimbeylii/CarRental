using AutoMapper;
using CarRental.Application.DTOs.Car;
using CarRental.Application.Exceptions;
using CarRental.Application.Interfaces.Repositories;
using CarRental.Application.Interfaces.Services;
using CarRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System.Linq.Expressions;
using Microsoft.Extensions.Primitives;

namespace CarRental.Application.Services;

public class CarService : ICarService
{
    private readonly ICarRepository _repository;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache;
    private CancellationTokenSource _cacheResetToken = new();

    public CarService(
        ICarRepository repository,
        IMapper mapper,
        IMemoryCache cache

        )
    {
        _repository = repository;

        _mapper = mapper;
        _cache = cache;
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
        var cacheKey =
            $"cars_" +
            $"model_{filter.Model}_" +
            $"brand_{filter.BrandId}_" +
            $"category_{filter.CategoryId}_" +
            $"minPrice_{filter.MinPrice}_" +
            $"maxPrice_{filter.MaxPrice}_" +
            $"available_{filter.IsAvailable}_" +
            $"page_{page}_" +
            $"take_{take}_" +
            $"sort_{sort}";

        if (_cache.TryGetValue(cacheKey, out IReadOnlyList<CarGetDto>? cachedCars))
        {
            return cachedCars!;
        }

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
            query = query.Where(x => x.DailyPrice >= filter.MinPrice.Value);

        if (filter.MaxPrice.HasValue)
            query = query.Where(x => x.DailyPrice <= filter.MaxPrice.Value);

        if (filter.IsAvailable.HasValue)
            query = query.Where(x => x.IsAvailable == filter.IsAvailable.Value);

        var cars = await query.ToListAsync();

        var result = _mapper.Map<IReadOnlyList<CarGetDto>>(cars);

        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        };

        cacheOptions.AddExpirationToken(
            new CancellationChangeToken(_cacheResetToken.Token));

        _cache.Set(cacheKey, result, cacheOptions);

        return result;
    }

    private void InvalidateCache()
    {
        _cacheResetToken.Cancel();
        _cacheResetToken.Dispose();

        _cacheResetToken = new CancellationTokenSource();
    }
    public async Task CreateAsync(CarCreateDto dto)
    {
        var entity = _mapper.Map<Car>(dto);

        _repository.Add(entity);

        await _repository.SaveChangesAsync();

        InvalidateCache();
    }

    public async Task UpdateAsync(CarUpdateDto dto)
    {
        var entity = await _repository.GetByIdAsync(dto.Id);

        if (entity == null)
            throw new NotFoundException(nameof(Car));

        _mapper.Map(dto, entity);

        _repository.Update(entity);

        await _repository.SaveChangesAsync();

        InvalidateCache();
    }

    public async Task DeleteAsync(long id)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity == null)
            throw new NotFoundException(nameof(Car));

        _repository.Delete(entity);

        await _repository.SaveChangesAsync();

        InvalidateCache();
    }
}