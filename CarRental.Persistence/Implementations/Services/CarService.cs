using AutoMapper;
using CarRental.Application.DTOs.Car;
using CarRental.Application.Interfaces.Repositories;
using CarRental.Application.Interfaces.Services;
using CarRental.Domain.Entities;

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
        var entity = await _repository.GetById(id);

        if (entity == null)
            throw new Exception("Car not found");

        return _mapper.Map<CarGetDto>(entity);
    }

    public async Task<IEnumerable<CarGetDto>> GetAllAsync()
    {
        var cars = _repository.GetAll();

        return _mapper.Map<IEnumerable<CarGetDto>>(cars);
    }


    public async Task CreateAsync(CarCreateDto dto)
    {
        var entity = _mapper.Map<Car>(dto);

        _repository.Add(entity);

        await _repository.SaveChangeAsync();
    }

    public async Task UpdateAsync(CarUpdateDto dto)
    {
        var entity = await _repository.GetById(dto.Id);

        if (entity == null)
            throw new Exception("Car not found");

        _mapper.Map(dto, entity);

        _repository.Update(entity);

        await _repository.SaveChangeAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var entity = await _repository.GetById(id);

        if (entity == null)
            throw new Exception("Car not found");

        _repository.Delete(entity);

        await _repository.SaveChangeAsync();
    }
}