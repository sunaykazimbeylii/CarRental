using CarRental.Application.Interfaces.Repositories;
using CarRental.Domain.Entities;
using CarRental.Persistence.Contexts;

namespace CarRental.Persistence.Implementations.Repositories
{
    internal class PaymentRepository:Repository<Payment>,IPaymentRepository
    {
        public PaymentRepository(AppDbContext context):base(context) { }
        
    }
}
