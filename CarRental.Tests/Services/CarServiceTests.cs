using AutoMapper;
using CarRental.Application.DTOs.Car;
using CarRental.Application.Interfaces.Repositories;
using CarRental.Application.Services;
using CarRental.Domain.Entities;
using CarRental.Domain.Enums;
using Moq;

namespace CarRental.Tests.Services;

public class CarServiceTests
{
    private readonly Mock<ICarRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CarService _service;

    public CarServiceTests()
    {
        _repositoryMock = new Mock<ICarRepository>();
        _mapperMock = new Mock<IMapper>();

        _service = new CarService(
            _repositoryMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Car()
    {
        // Arrange

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
            .Setup(x => x.GetById(1))
            .ReturnsAsync(car);

        _mapperMock
            .Setup(x => x.Map<CarGetDto>(car))
            .Returns(dto);

        // Act

        var result = await _service.GetByIdAsync(1);

        // Assert

        Assert.NotNull(result);
        Assert.Equal("BMW", result.Model);

        _repositoryMock.Verify(x => x.GetById(1), Times.Once);

        _mapperMock.Verify(x => x.Map<CarGetDto>(car), Times.Once);
    }
}