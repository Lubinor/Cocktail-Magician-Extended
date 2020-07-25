using CocktailMagician.Data.Configuration;
using CocktailMagician.Data.Seeder;
using CocktailMagician.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CocktailMagician.Data
{
    public class CocktailMagicianContext : IdentityDbContext<User, Role, int>
    {
        public CocktailMagicianContext(DbContextOptions<CocktailMagicianContext> options)
            : base(options)
        {

        }

        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Cocktail> Cocktails { get; set; }
        public DbSet<Bar> Bars { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<IngredientsCocktails> IngredientsCocktails { get; set; }
        public DbSet<BarsCocktails> BarsCocktails { get; set; }
        public DbSet<CocktailsUsersReviews> CocktailsUsersReviews { get; set; }
        public DbSet<BarsUsersReviews> BarsUsersReviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new IngredientsConfig());
            modelBuilder.ApplyConfiguration(new CocktailsConfig());
            modelBuilder.ApplyConfiguration(new BarsConfig());
            modelBuilder.ApplyConfiguration(new CitiesConfig());
            modelBuilder.ApplyConfiguration(new IngredientsCocktailsConfig());
            modelBuilder.ApplyConfiguration(new BarsCocktailsConfig());
            modelBuilder.ApplyConfiguration(new CocktailsUsersReviewsConfig());
            modelBuilder.ApplyConfiguration(new BarsUsersReviewsConfig());

            modelBuilder.Seeder();

            base.OnModelCreating(modelBuilder);
        }
    }
}
