using CarRental.Application.Interfaces.Repositories;
using CarRental.Domain.Entities;
using Moq;

namespace CarRental.Tests.Services
{
    public class CarServiceTests
    {
        private readonly Mock<ICarRepository> _repositoryMock;


        public CarServiceTests()
        {
            _repositoryMock = new Mock<ICarRepository>();
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


            _repositoryMock
                .Setup(x => x.GetById(1))
                .ReturnsAsync(car);



            // Act

            var result = await _repositoryMock.Object
                .GetById(1);



            // Assert

            Assert.NotNull(result);
            Assert.Equal("BMW", result.Model);
        }
    }
}