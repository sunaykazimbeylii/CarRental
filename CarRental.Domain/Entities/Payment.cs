using CarRental.Domain.Entities.Common;
using CarRental.Domain.Enums;

namespace CarRental.Domain.Entities
{
    public class Payment:BaseEntity
    {
        public long RentalId { get; set; }

        public Rental Rental { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

     
        public PaymentMethod PaymentMethod { get; set; }

        public PaymentStatus Status { get; set; }


    }
}
