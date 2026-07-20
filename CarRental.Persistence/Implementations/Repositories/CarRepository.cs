using CarRental.Application.Interfaces.Repositories;
using CarRental.Domain.Entities;
using CarRental.Persistence.Contexts;
using CarRentalSystem.API.Implementations.Repository.Generic;

namespace CarRental.Persistence.Implementations.Repositories
{
    internal class CarRepository:Repository<Car>,ICarRepository
    {
        public CarRepository(AppDbContext context ):base(context) { }
       
    }
}
