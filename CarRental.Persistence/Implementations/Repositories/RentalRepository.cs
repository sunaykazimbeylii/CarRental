using CarRental.Application.Interfaces.Repositories;
using CarRental.Domain.Entities;
using CarRental.Persistence.Contexts;

namespace CarRental.Persistence.Implementations.Repositories
{
    public class RentalRepository:Repository<Rental>,IRentalRepository
    {
        public RentalRepository(AppDbContext context) : base(context) { }
      
    }
}
