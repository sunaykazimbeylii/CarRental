using CarRental.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.Domain.Entities
{
    public class Feature:BaseEntity
    {
        public string Name { get; set; }

        public ICollection<CarFeature> CarFeatures { get; set; }
    }
}
