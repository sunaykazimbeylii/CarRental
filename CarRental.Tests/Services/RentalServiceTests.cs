using AutoMapper;
using CarRental.Application.DTOs.Rental;
using CarRental.Application.Exceptions;
using CarRental.Application.Services;
using CarRental.Domain.Entities;
using CarRental.Domain.Enums;
using CarRental.Persistence.Contexts;
using CarRental.Persistence.Implementations.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CarRental.Tests.Services;

public class RentalServiceTests
{
    public RentalServiceTests()
    {
        SQLitePCL.Batteries.Init();
    }

    [Fact]
    public async Task CreateAsync_ShouldRollback_WhenExceptionOccurs()
    {
        await using var connection = new SqliteConnection("DataSource=:memory:");
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

        var user = new AppUser
        {
            UserName = "testuser",
            Email = "test@test.com",
            Name = "Test",
            Surname = "User"
        };

        context.Users.Add(user);

        await context.SaveChangesAsync();

        var car = new Car
        {
            Model = "BMW X5",
            Year = 2024,
            PlateNumber = "10-AA-001",
            DailyPrice = 100,
            Mileage = 1000,
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

        var service = new RentalService(
            new RentalRepository(context),
            new CarRepository(context),
            new PaymentRepository(context),
            new Mock<IMapper>().Object,
            context);

        var dto = new RentalCreateDto(
            user.Id,
            car.Id,
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddDays(3),
            PaymentMethod.CreditCard
        );

        await Assert.ThrowsAsync<TransactionRollbackTestException>(
            () => service.CreateAsync(dto));

        Assert.Empty(context.Rentals);
        Assert.Empty(context.Payments);

        // Rollback-dan sonra EF Core Change Tracker-i təmizləyirik.
        context.ChangeTracker.Clear();

        // Məlumatı database-dən yenidən oxuyuruq.
        var dbCar = await context.Cars.FindAsync(car.Id);

        Assert.NotNull(dbCar);
        Assert.True(dbCar!.IsAvailable);
    }
}