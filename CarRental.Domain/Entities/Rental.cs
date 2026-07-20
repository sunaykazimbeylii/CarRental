using CarRental.Domain.Entities.Common;
using CarRental.Domain.Enums;

namespace CarRental.Domain.Entities
{
    public class Rental:BaseEntity
    {
        public long UserId { get; set; }

        public AppUser User { get; set; }

        public long CarId { get; set; }

        public Car Car { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal TotalPrice { get; set; }

        public RentalStatus Status { get; set; }

        public Payment Payment { get; set; }
    }
}
