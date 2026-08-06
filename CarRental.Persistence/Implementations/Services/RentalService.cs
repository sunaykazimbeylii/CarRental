using AutoMapper;
using CarRental.Application.DTOs.Rental;
using CarRental.Application.Exceptions;
using CarRental.Application.Interfaces.Repositories;
using CarRental.Application.Interfaces.Services;
using CarRental.Domain.Entities;
using CarRental.Domain.Enums;
using CarRental.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Application.Services;

public class RentalService : IRentalService
{
    private readonly IRentalRepository _rentalRepository;
    private readonly ICarRepository _carRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;

    public RentalService(
        IRentalRepository rentalRepository,
        ICarRepository carRepository,
        IPaymentRepository paymentRepository,
        IMapper mapper,
        AppDbContext context)
    {
        _rentalRepository = rentalRepository;
        _carRepository = carRepository;
        _paymentRepository = paymentRepository;
        _mapper = mapper;
        _context = context;
    }

    public async Task CreateAsync(RentalCreateDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var car = await _carRepository.GetByIdAsync(dto.CarId);

            if (car is null)
                throw new NotFoundException(nameof(Car));

            if (!car.IsAvailable)
                throw new CarNotAvailableException();

            int days = (dto.EndDate - dto.StartDate).Days;

            if (days <= 0)
                throw new InvalidRentalPeriodException();

            if (dto.StartDate.Date < DateTime.UtcNow.Date)
                throw new PastRentalDateException();

            decimal totalPrice = days * car.DailyPrice;

            Rental rental = new()
            {
                UserId = dto.UserId,
                CarId = dto.CarId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                TotalPrice = totalPrice,
                Status = RentalStatus.Pending
            };

            _rentalRepository.Add(rental);

            car.IsAvailable = false;
            _carRepository.Update(car);

            Payment payment = new()
            {
                Rental = rental,
                Amount = totalPrice,
                PaymentDate = DateTime.UtcNow,
                PaymentMethod = dto.PaymentMethod,
                Status = PaymentStatus.Pending
            };

            _paymentRepository.Add(payment);

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<List<RentalGetDto>> GetAllAsync()
    {
        var rentals = await _rentalRepository
    .GetAll(
        includes: new[]
        {
            nameof(Rental.User),
            nameof(Rental.Car)
        })
    .ToListAsync();

        return _mapper.Map<List<RentalGetDto>>(rentals);
    }

    public async Task<RentalGetDto> GetByIdAsync(long id)
    {
        var rental = await _rentalRepository.GetByIdAsync(
            id,
            nameof(Rental.User),
            nameof(Rental.Car));

        if (rental is null)
            throw new NotFoundException(nameof(Rental));

        return _mapper.Map<RentalGetDto>(rental);
    }
}