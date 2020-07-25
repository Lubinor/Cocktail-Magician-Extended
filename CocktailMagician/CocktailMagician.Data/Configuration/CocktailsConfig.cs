using CocktailMagician.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CocktailMagician.Data.Configuration
{
    public class CocktailsConfig : IEntityTypeConfiguration<Cocktail>
    {
        public void Configure(EntityTypeBuilder<Cocktail> builder)
        {
            builder.HasKey(cocktail => cocktail.Id);

            builder.Property(cocktail => cocktail.Name)
                .IsRequired()
                .HasMaxLength(30);

            builder.HasOne(cocktail => cocktail.Creator)
                .WithMany(user => user.CreatedCocktails)
                .HasForeignKey(key => key.CreatorId);

            //builder.Property(cocktail => cocktail.UserId)
            //    .IsRequired();
        }
    }
}