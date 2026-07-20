using CarRental.Domain.Entities.Common;

namespace CarRental.Domain.Entities
{
    public class Review : BaseEntity
    {
        public int Rating { get; set; }

        public string Comment { get; set; }

        public long UserId { get; set; }

        public AppUser User { get; set; }

        public long CarId { get; set; }

        public Car Car { get; set; }
    }
}
