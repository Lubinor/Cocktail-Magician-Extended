using CocktailMagician.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CocktailMagician.Data.Configuration
{
    public class IngredientsCocktailsConfig : IEntityTypeConfiguration<IngredientsCocktails>
    {
        public void Configure(EntityTypeBuilder<IngredientsCocktails> builder)
        {
            builder.HasKey(ic => new { ic.IngredientId, ic.CocktailId });
        }
    }
}