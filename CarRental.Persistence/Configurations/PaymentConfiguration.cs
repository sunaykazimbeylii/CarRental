using CarRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRental.Persistence.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.Property(x => x.Amount)
                   .HasPrecision(18, 2)
                   .IsRequired();

            builder.Property(x => x.PaymentDate)
                   .IsRequired();

            builder.Property(x => x.PaymentMethod)
                   .IsRequired();

            builder.Property(x => x.Status)
                   .IsRequired();

            builder.HasOne(x => x.Rental)
                   .WithOne(x => x.Payment)
                   .HasForeignKey<Payment>(x => x.RentalId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}