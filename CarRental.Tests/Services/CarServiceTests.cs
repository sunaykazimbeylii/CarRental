using AutoMapper;
using CarRental.Application.DTOs.Car;
using CarRental.Application.Interfaces.Repositories;
using CarRental.Application.Services;
using CarRental.Domain.Entities;
using CarRental.Domain.Enums;
using CarRental.Persistence.Contexts;
using CarRental.Persistence.Implementations.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System.Linq.Expressions;

namespace CarRental.Tests.Services;

public class CarServiceTests
{
    private readonly Mock<ICarRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CarService _service;
    private readonly IMemoryCache _cache;

    public CarServiceTests()
    {
        _repositoryMock = new Mock<ICarRepository>();
        _mapperMock = new Mock<IMapper>();

        _cache = new MemoryCache(new MemoryCacheOptions());

        _service = new CarService(
            _repositoryMock.Object,
            _mapperMock.Object,
            _cache);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Car()
    {
    
        var car = new Car
        {
            Id = 1,
            Model = "BMW"
        };

        var dto = new CarGetDto(
            1,
            "BMW",
            2024,
            "10-AA-001",
            150,
            10000,
            FuelType.Petrol,
            Transmission.Automatic,
            true,
            "BMW",
            "Sedan",
            "Baku",
            "Black"
        );

        _repositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(car);

        _mapperMock
            .Setup(x => x.Map<CarGetDto>(car))
            .Returns(dto);

    
        var result = await _service.GetByIdAsync(1);

       
        Assert.NotNull(result);
        Assert.Equal("BMW", result.Model);

        _repositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);

        _mapperMock.Verify(
            x => x.Map<CarGetDto>(car),
            Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_Data_From_Cache()
    {
      
        var filter = new CarFilterDto(
            null,
            null,
            null,
            null,
            null,
            null);

        var cachedCars = new List<CarGetDto>
        {
            new CarGetDto(
                1,
                "BMW",
                2024,
                "10-AA-001",
                150,
                10000,
                FuelType.Petrol,
                Transmission.Automatic,
                true,
                "BMW",
                "Sedan",
                "Baku",
                "Black")
        };

        var cacheKey =
            $"cars_" +
            $"model_{filter.Model}_" +
            $"brand_{filter.BrandId}_" +
            $"category_{filter.CategoryId}_" +
            $"minPrice_{filter.MinPrice}_" +
            $"maxPrice_{filter.MaxPrice}_" +
            $"available_{filter.IsAvailable}_" +
            $"page_1_" +
            $"take_10_" +
            $"sort_";

        _cache.Set(
            cacheKey,
            (IReadOnlyList<CarGetDto>)cachedCars);

        var result = await _service.GetAllAsync(
            filter,
            1,
            10,
            null);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("BMW", result[0].Model);

        _repositoryMock.Verify(
            x => x.GetAll(
                It.IsAny<Expression<Func<Car, bool>>?>(),
                It.IsAny<Expression<Func<Car, object>>?>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string[]>()),
            Times.Never);
    }

    [Fact]
    public async Task CreateAsync_Should_Invalidate_Car_Cache()
    {
      
        SQLitePCL.Batteries.Init();

        await using var connection =
            new SqliteConnection("DataSource=:memory:");

        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        await using var context = new AppDbContext(options);

        await context.Database.EnsureCreatedAsync();

        var brand = new Brand
        {
            Name = "BMW",
            Country = "Germany"
        };

        var category = new Category
        {
            Name = "Sedan"
        };

        var branch = new Branch
        {
            Name = "Main",
            City = "Baku",
            Address = "Nizami 1"
        };

        var color = new Color
        {
            Name = "Black",
            HexCode = "#000000"
        };

        context.Brands.Add(brand);
        context.Categories.Add(category);
        context.Branches.Add(branch);
        context.Colors.Add(color);

        await context.SaveChangesAsync();

        var existingCar = new Car
        {
            Model = "BMW",
            Year = 2024,
            PlateNumber = "10-AA-001",
            DailyPrice = 150,
            Mileage = 10000,
            FuelType = FuelType.Petrol,
            Transmission = Transmission.Automatic,
            IsAvailable = true,
            BrandId = brand.Id,
            CategoryId = category.Id,
            BranchId = branch.Id,
            ColorId = color.Id
        };

        context.Cars.Add(existingCar);

        await context.SaveChangesAsync();

        var repository = new CarRepository(context);

        var mapperMock = new Mock<IMapper>();

        var service = new CarService(
            repository,
            mapperMock.Object,
            _cache);

        var filter = new CarFilterDto(
            null,
            null,
            null,
            null,
            null,
            null);

     
        mapperMock
            .Setup(x => x.Map<IReadOnlyList<CarGetDto>>(
                It.IsAny<List<Car>>()))
            .Returns((List<Car> cars) =>
                cars.Select(x => new CarGetDto(
                    x.Id,
                    x.Model,
                    x.Year,
                    x.PlateNumber,
                    x.DailyPrice,
                    x.Mileage,
                    x.FuelType,
                    x.Transmission,
                    x.IsAvailable,
                    "BMW",
                    "Sedan",
                    "Baku",
                    "Black"
                )).ToList());

        var newCarDto = new CarCreateDto(
            "Mercedes",
            2025,
            "10-BB-002",
            200,
            5000,
            FuelType.Petrol,
            Transmission.Automatic,
            true,
            brand.Id,
            category.Id,
            branch.Id,
            color.Id
        );

        var newCar = new Car
        {
            Model = "Mercedes",
            Year = 2025,
            PlateNumber = "10-BB-002",
            DailyPrice = 200,
            Mileage = 5000,
            FuelType = FuelType.Petrol,
            Transmission = Transmission.Automatic,
            IsAvailable = true,
            BrandId = brand.Id,
            CategoryId = category.Id,
            BranchId = branch.Id,
            ColorId = color.Id
        };

        mapperMock
            .Setup(x => x.Map<Car>(newCarDto))
            .Returns(newCar);

        var firstResult = await service.GetAllAsync(
            filter,
            1,
            10,
            null);

        await service.CreateAsync(newCarDto);

    
        var secondResult = await service.GetAllAsync(
            filter,
            1,
            10,
            null);


        Assert.NotNull(firstResult);
        Assert.NotNull(secondResult);

        Assert.Single(firstResult);
        Assert.Equal(2, secondResult.Count);

        Assert.Contains(
            secondResult,
            x => x.Model == "BMW");

        Assert.Contains(
            secondResult,
            x => x.Model == "Mercedes");
    }
    [Fact]
    public async Task GetAllAsync_Should_Get_Data_From_Repository_When_Cache_Is_Empty()
    {
        // Arrange
        var filter = new CarFilterDto(
            null,
            null,
            null,
            null,
            null,
            null);

        var cars = new List<Car>
    {
        new Car
        {
            Id = 1,
            Model = "BMW",
            Year = 2024,
            PlateNumber = "10-AA-001",
            DailyPrice = 150,
            Mileage = 10000,
            FuelType = FuelType.Petrol,
            Transmission = Transmission.Automatic,
            IsAvailable = true
        }
    };

        var carDtos = new List<CarGetDto>
    {
        new CarGetDto(
            1,
            "BMW",
            2024,
            "10-AA-001",
            150,
            10000,
            FuelType.Petrol,
            Transmission.Automatic,
            true,
            "BMW",
            "Sedan",
            "Baku",
            "Black")
    };

        _repositoryMock
            .Setup(x => x.GetAll(
                It.IsAny<Expression<Func<Car, bool>>?>(),
                It.IsAny<Expression<Func<Car, object>>?>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string[]>()))
            .Returns(cars.AsQueryable());

        _mapperMock
            .Setup(x => x.Map<IReadOnlyList<CarGetDto>>(
                It.IsAny<List<Car>>()))
            .Returns(carDtos);

        // Act
        var result = await _service.GetAllAsync(
            filter,
            1,
            10,
            null);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("BMW", result[0].Model);

        // Cache boş olduğu üçün Repository çağırılmalıdır
        _repositoryMock.Verify(
            x => x.GetAll(
                It.IsAny<Expression<Func<Car, bool>>?>(),
                It.IsAny<Expression<Func<Car, object>>?>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string[]>()),
            Times.Once);
    }
    [Fact]
    public async Task UpdateAsync_Should_Invalidate_Car_Cache()
    {
       
        SQLitePCL.Batteries.Init();

        await using var connection =
            new SqliteConnection("DataSource=:memory:");

        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        await using var context = new AppDbContext(options);

        await context.Database.EnsureCreatedAsync();

        var brand = new Brand
        {
            Name = "BMW",
            Country = "Germany"
        };

        var category = new Category
        {
            Name = "Sedan"
        };

        var branch = new Branch
        {
            Name = "Main",
            City = "Baku",
            Address = "Nizami 1"
        };

        var color = new Color
        {
            Name = "Black",
            HexCode = "#000000"
        };

        context.Brands.Add(brand);
        context.Categories.Add(category);
        context.Branches.Add(branch);
        context.Colors.Add(color);

        await context.SaveChangesAsync();

        var car = new Car
        {
            Model = "BMW",
            Year = 2024,
            PlateNumber = "10-AA-001",
            DailyPrice = 150,
            Mileage = 10000,
            FuelType = FuelType.Petrol,
            Transmission = Transmission.Automatic,
            IsAvailable = true,
            BrandId = brand.Id,
            CategoryId = category.Id,
            BranchId = branch.Id,
            ColorId = color.Id
        };

        context.Cars.Add(car);
        await context.SaveChangesAsync();

        var repository = new CarRepository(context);
        var mapperMock = new Mock<IMapper>();

        var service = new CarService(
            repository,
            mapperMock.Object,
            _cache);

        var filter = new CarFilterDto(
            null,
            null,
            null,
            null,
            null,
            null);

        mapperMock
            .Setup(x => x.Map<IReadOnlyList<CarGetDto>>(
                It.IsAny<List<Car>>()))
            .Returns((List<Car> cars) =>
                cars.Select(x => new CarGetDto(
                    x.Id,
                    x.Model,
                    x.Year,
                    x.PlateNumber,
                    x.DailyPrice,
                    x.Mileage,
                    x.FuelType,
                    x.Transmission,
                    x.IsAvailable,
                    "BMW",
                    "Sedan",
                    "Baku",
                    "Black"
                )).ToList());

        var updateDto = new CarUpdateDto(
            car.Id,
            "Mercedes",
            2025,
            "10-BB-002",
            200,
            5000,
            FuelType.Diesel,
            Transmission.Manual,
            true,
            brand.Id,
            category.Id,
            branch.Id,
            color.Id);

        var firstResult = await service.GetAllAsync(
            filter,
            1,
            10,
            null);

        await service.UpdateAsync(updateDto);

    
        var secondResult = await service.GetAllAsync(
            filter,
            1,
            10,
            null);

     
        Assert.Single(firstResult);
        Assert.Single(secondResult);

        Assert.Equal("BMW", firstResult[0].Model);
        Assert.Equal("Mercedes", secondResult[0].Model);
    }
    [Fact]
    public async Task DeleteAsync_Should_Invalidate_Car_Cache()
    {
     
        SQLitePCL.Batteries.Init();

        await using var connection =
            new SqliteConnection("DataSource=:memory:");

        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        await using var context = new AppDbContext(options);

        await context.Database.EnsureCreatedAsync();

        var brand = new Brand
        {
            Name = "BMW",
            Country = "Germany"
        };

        var category = new Category
        {
            Name = "Sedan"
        };

        var branch = new Branch
        {
            Name = "Main",
            City = "Baku",
            Address = "Nizami 1"
        };

        var color = new Color
        {
            Name = "Black",
            HexCode = "#000000"
        };

        context.Brands.Add(brand);
        context.Categories.Add(category);
        context.Branches.Add(branch);
        context.Colors.Add(color);

        await context.SaveChangesAsync();

        var car = new Car
        {
            Model = "BMW",
            Year = 2024,
            PlateNumber = "10-AA-001",
            DailyPrice = 150,
            Mileage = 10000,
            FuelType = FuelType.Petrol,
            Transmission = Transmission.Automatic,
            IsAvailable = true,
            BrandId = brand.Id,
            CategoryId = category.Id,
            BranchId = branch.Id,
            ColorId = color.Id
        };

        context.Cars.Add(car);
        await context.SaveChangesAsync();

        var repository = new CarRepository(context);
        var mapperMock = new Mock<IMapper>();

        var service = new CarService(
            repository,
            mapperMock.Object,
            _cache);

        var filter = new CarFilterDto(
            null,
            null,
            null,
            null,
            null,
            null);

        mapperMock
            .Setup(x => x.Map<IReadOnlyList<CarGetDto>>(
                It.IsAny<List<Car>>()))
            .Returns((List<Car> cars) =>
                cars.Select(x => new CarGetDto(
                    x.Id,
                    x.Model,
                    x.Year,
                    x.PlateNumber,
                    x.DailyPrice,
                    x.Mileage,
                    x.FuelType,
                    x.Transmission,
                    x.IsAvailable,
                    "BMW",
                    "Sedan",
                    "Baku",
                    "Black"
                )).ToList());

    
        var firstResult = await service.GetAllAsync(
            filter,
            1,
            10,
            null);

        await service.DeleteAsync(car.Id);

  
        var secondResult = await service.GetAllAsync(
            filter,
            1,
            10,
            null);

 
        Assert.Single(firstResult);
        Assert.Empty(secondResult);
    }
}