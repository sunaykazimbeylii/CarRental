using CarRental.Application.Interfaces.Repositories;
using CarRental.Domain.Entities;
using CarRental.Persistence.Contexts;
using CarRentalSystem.API.Implementations.Repository.Generic;

namespace CarRental.Persistence.Implementations.Repositories
{
    internal class BrandRepository : Repository<Brand>,IBrandRepository
    {
        public BrandRepository(AppDbContext context) : base(context) { }
       
    }
}
