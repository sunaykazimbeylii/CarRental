using CarRental.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.Domain.Entities
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }

        public ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}
