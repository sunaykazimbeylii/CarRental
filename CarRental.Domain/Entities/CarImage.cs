using CarRental.Domain.Entities.Common;

namespace CarRental.Domain.Entities
{
    public class CarImage : BaseEntity
    {
        public string ImageUrl { get; set; }

        public long CarId { get; set; }

        public Car Car { get; set; }
    }
}
