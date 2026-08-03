using CarRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRental.Persistence.Configurations
{
    public class FeatureConfiguration : IEntityTypeConfiguration<Feature>
    {
        public void Configure(EntityTypeBuilder<Feature> builder)
        {
            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasMany(x => x.CarFeatures)
                   .WithOne(x => x.Feature)
                   .HasForeignKey(x => x.FeatureId);
        }
    }
}
