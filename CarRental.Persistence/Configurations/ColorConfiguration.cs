using CarRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRental.Persistence.Configurations
{
    public class ColorConfiguration : IEntityTypeConfiguration<Color>
    {
        public void Configure(EntityTypeBuilder<Color> builder)
        {
            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.HexCode)
                   .IsRequired()
                   .HasMaxLength(7);

            builder.HasIndex(x => x.Name)
                   .IsUnique();

            builder.HasMany(x => x.Cars)
        .WithOne(x => x.Color)
        .HasForeignKey(x => x.ColorId)
        .OnDelete(DeleteBehavior.Restrict);
        }
    }
}