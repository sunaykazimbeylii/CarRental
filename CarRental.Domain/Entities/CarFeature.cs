namespace CarRental.Domain.Entities
{
    public class CarFeature
    {
        public long CarId { get; set; }
        public Car Car { get; set; }

        public long FeatureId { get; set; }
        public Feature Feature { get; set; }
    }
}
