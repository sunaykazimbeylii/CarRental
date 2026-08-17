using CarRental.Application.Interfaces.Repositories;
using CarRental.Domain.Entities;
using CarRental.Persistence.Contexts;

namespace CarRental.Persistence.Implementations.Repositories;

public class CarImageRepository : Repository<CarImage>, ICarImageRepository
{
    public CarImageRepository(AppDbContext context)
        : base(context)
    {
    }
}