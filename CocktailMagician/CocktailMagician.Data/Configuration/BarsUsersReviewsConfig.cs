using CocktailMagician.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CocktailMagician.Data.Configuration
{
    public class BarsUsersReviewsConfig : IEntityTypeConfiguration<BarsUsersReviews>
    {
        public void Configure(EntityTypeBuilder<BarsUsersReviews> builder)
        {
            builder.HasKey(review => new { review.BarId, review.UserId });

            builder.Property(review => review.Rating)
                .IsRequired();

            builder.Property(review => review.Comment)
                .HasMaxLength(500);
        }
    }
}