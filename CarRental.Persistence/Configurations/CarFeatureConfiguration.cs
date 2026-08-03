using CarRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRental.Persistence.Configurations
{
    public class CarFeatureConfiguration : IEntityTypeConfiguration<CarFeature>
    {
        public void Configure(EntityTypeBuilder<CarFeature> builder)
        {
            builder.HasKey(x => new { x.CarId, x.FeatureId });

            builder.HasOne(x => x.Car)
                   .WithMany(x => x.CarFeatures)
                   .HasForeignKey(x => x.CarId);

            builder.HasOne(x => x.Feature)
                   .WithMany(x => x.CarFeatures)
                   .HasForeignKey(x => x.FeatureId);
        }
    }
}
