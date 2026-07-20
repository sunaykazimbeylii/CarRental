using CarRental.Domain.Entities.Common;
using CarRental.Domain.Enums;

namespace CarRental.Domain.Entities
{
    public class Car:BaseEntity
    {
        public string Model { get; set; }

        public int Year { get; set; }

        public string PlateNumber { get; set; }

        public decimal DailyPrice { get; set; }

        public int Mileage { get; set; }

        public FuelType FuelType { get; set; }

        public Transmission Transmission { get; set; }

        public bool IsAvailable { get; set; }

        public Brand Brand { get; set; }

        public long BrandId { get; set; }

        public Category Category { get; set; }

        public long CategoryId { get; set; }

        public Branch Branch { get; set; }

        public long BranchId { get; set; }

        public Color Color { get; set; }

        public long CoorId { get; set; }

        public ICollection<Rental> Rentals { get; set; }

        public ICollection<CarImage> Images { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
}
