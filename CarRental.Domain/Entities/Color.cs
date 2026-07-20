using CarRental.Domain.Entities.Common;

namespace CarRental.Domain.Entities
{
    public class Color:BaseEntity
    {
            public string Name { get; set; }

            public string HexCode { get; set; }

            public ICollection<Car> Cars { get; set; }
        
    }
}
