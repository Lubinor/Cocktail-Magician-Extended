using CocktailMagician.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CocktailMagician.Data.Configuration
{
    public class CocktailsUsersReviewsConfig : IEntityTypeConfiguration<CocktailsUsersReviews>
    {
        public void Configure(EntityTypeBuilder<CocktailsUsersReviews> builder)
        {
            builder.HasKey(review => new { review.CocktailId, review.UserId });

            builder.Property(review => review.Rating)
                .IsRequired();

            builder.Property(review => review.Comment)
                .HasMaxLength(500);
        }
    }
}