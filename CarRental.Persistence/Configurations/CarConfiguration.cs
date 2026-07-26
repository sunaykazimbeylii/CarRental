using CarRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRental.Persistence.Configurations
{
    public class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Model)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.PlateNumber)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(x => x.Year)
                   .IsRequired();

            builder.Property(x => x.DailyPrice)
                   .HasPrecision(18, 2)
                   .IsRequired();

            builder.Property(x => x.Mileage)
                   .IsRequired();

            builder.Property(x => x.FuelType)
                   .IsRequired();

            builder.Property(x => x.Transmission)
                   .IsRequired();

            builder.Property(x => x.IsAvailable)
                   .HasDefaultValue(true);

            builder.HasIndex(x => x.PlateNumber)
                   .IsUnique();

            builder.HasOne(x => x.Brand)
                   .WithMany(x => x.Cars)
                   .HasForeignKey(x => x.BrandId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Category)
                   .WithMany(x => x.Cars)
                   .HasForeignKey(x => x.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Branch)
                   .WithMany(x => x.Cars)
                   .HasForeignKey(x => x.BranchId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Color)
                   .WithMany(x => x.Cars)
                   .HasForeignKey(x => x.ColorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Images)
                   .WithOne(x => x.Car)
                   .HasForeignKey(x => x.CarId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Rentals)
                   .WithOne(x => x.Car)
                   .HasForeignKey(x => x.CarId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Reviews)
                   .WithOne(x => x.Car)
                   .HasForeignKey(x => x.CarId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}