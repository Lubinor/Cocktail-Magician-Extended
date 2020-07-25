using CocktailMagician.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CocktailMagician.Data.Configuration
{
    public class BarsConfig : IEntityTypeConfiguration<Bar>
    {
        public void Configure(EntityTypeBuilder<Bar> builder)
        {
            builder.HasKey(bar => bar.Id);

            builder.Property(bar => bar.Name)
                .IsRequired()
                .HasMaxLength(30);

            builder.HasOne(bar => bar.City)
                .WithMany(city => city.Bars)
                .HasForeignKey(key => key.CityId);

            builder.Property(bar => bar.CityId)
                .IsRequired();

            builder.Property(bar => bar.Address)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(bar => bar.Phone)
                .HasMaxLength(20)
                .IsRequired();
        }
    }
}