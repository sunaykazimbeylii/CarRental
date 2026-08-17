using CarRental.Application.Interfaces.Repositories;
using CarRental.Domain.Entities;
using CarRental.Persistence.Contexts;

namespace CarRental.Persistence.Implementations.Repositories
{
    public class CarRepository:Repository<Car>,ICarRepository
    {
        public CarRepository(AppDbContext context ):base(context) { }
       
    }
}
