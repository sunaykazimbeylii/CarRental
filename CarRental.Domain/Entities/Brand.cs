using CarRental.Domain.Entities.Common;

namespace CarRental.Domain.Entities
{
    public class Brand:BaseEntity
    {
        public string Name { get; set; }

        public string Country { get; set; }

        public ICollection<Car> Cars { get; set; }
    }
}
