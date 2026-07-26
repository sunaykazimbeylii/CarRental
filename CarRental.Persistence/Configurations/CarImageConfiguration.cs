using CarRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRental.Persistence.Configurations
{
    public class CarImageConfiguration : IEntityTypeConfiguration<CarImage>
    {
        public void Configure(EntityTypeBuilder<CarImage> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ImageUrl)
                   .IsRequired()
                   .HasMaxLength(1000);

            builder.HasOne(x => x.Car)
                   .WithMany(x => x.Images)
                   .HasForeignKey(x => x.CarId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}