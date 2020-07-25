using CocktailMagician.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CocktailMagician.Data.Configuration
{
    public class BarsCocktailsConfig : IEntityTypeConfiguration<BarsCocktails>
    {
        public void Configure(EntityTypeBuilder<BarsCocktails> builder)
        {
            builder.HasKey(bc => new { bc.BarId, bc.CocktailId });
        }
    }
}