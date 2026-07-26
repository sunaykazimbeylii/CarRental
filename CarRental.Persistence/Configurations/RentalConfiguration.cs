using CarRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRental.Persistence.Configurations
{
    public class RentalConfiguration : IEntityTypeConfiguration<Rental>
    {
        public void Configure(EntityTypeBuilder<Rental> builder)
        {
            builder.Property(x => x.StartDate)
                   .IsRequired();

            builder.Property(x => x.EndDate)
                   .IsRequired();

            builder.Property(x => x.TotalPrice)
                   .HasPrecision(18, 2)
                   .IsRequired();

            builder.Property(x => x.Status)
                   .IsRequired();

            builder.HasOne(x => x.User)
        .WithMany()
        .HasForeignKey(x => x.UserId)
        .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Car)
                   .WithMany(x => x.Rentals)
                   .HasForeignKey(x => x.CarId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}