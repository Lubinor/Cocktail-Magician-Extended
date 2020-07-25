using CocktailMagician.Models;
using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CocktailMagician.Tests.ServiceTests.DTOMappersTests
{
    public class CocktailReviewMapper_Should
    {
        [TestMethod]
        public void CorrectReturnInstanceType_ToCocktailReviewDTO()
        {
            //Arrange
            var sut = new CocktailReviewMapper();

            var options = Utils.GetOptions(nameof(CorrectReturnInstanceType_ToCocktailReviewDTO));
            var cocktail = new Cocktail { Id = 1, Name = "Mojito" };
            var user = new User { Id = 2, UserName = "Tom" };
            var review = new CocktailsUsersReviews
            {
                CocktailId = 1,
                Cocktail = cocktail,
                UserId = 2,
                User = user,
                Comment = "Top!",
                Rating = 4
            };

            //Act
            var result = sut.MapToCocktailReviewDTO(review);

            //Assert
            Assert.IsInstanceOfType(result, typeof(CocktailReviewDTO));
        }

        [TestMethod]
        public void CorrectMapping_ToCocktailReviewDTO()
        {
            //Arrange
            var sut = new CocktailReviewMapper();

            var options = Utils.GetOptions(nameof(CorrectMapping_ToCocktailReviewDTO));
            var cocktail = new Cocktail { Id = 1, Name = "Mojito" };
            var user = new User { Id = 2, UserName = "Tom" };
            var review = new CocktailsUsersReviews
            {
                CocktailId = 1,
                Cocktail = cocktail,
                UserId = 2,
                User = user,
                Comment = "Top!",
                Rating = 4
            };

            //Act
            var result = sut.MapToCocktailReviewDTO(review);

            //Assert
            Assert.AreEqual(review.CocktailId, result.CocktailId);
            Assert.AreEqual(review.UserId, result.AuthorId);
            Assert.AreEqual(review.Cocktail.Name, result.CocktailName);
            Assert.AreEqual(review.User.UserName, result.Author);
            Assert.AreEqual(review.Comment, result.Comment);
            Assert.AreEqual(review.Rating, result.Rating);
        }

        [TestMethod]
        public void CorrectReturnInstanceType_ToCocktailReview()
        {
            //Arrange
            var sut = new CocktailReviewMapper();

            var options = Utils.GetOptions(nameof(CorrectReturnInstanceType_ToCocktailReview));

            var reviewDTO = new CocktailReviewDTO
            {
                CocktailId = 1,
                AuthorId = 2,
                Comment = "Top!",
                Rating = 4,
            };

            //Act
            var result = sut.MapToCocktailReview(reviewDTO);

            //Assert
            Assert.IsInstanceOfType(result, typeof(CocktailsUsersReviews));
        }

        [TestMethod]
        public void CorrectMapping_ToCocktailReview()
        {
            //Arrange
            var sut = new CocktailReviewMapper();

            var options = Utils.GetOptions(nameof(CorrectMapping_ToCocktailReview));

            var reviewDTO = new CocktailReviewDTO
            {
                CocktailId = 1,
                AuthorId = 2,
                Comment = "Top!",
                Rating = 4,
            };

            //Act
            var result = sut.MapToCocktailReview(reviewDTO);

            //Assert
            Assert.AreEqual(reviewDTO.CocktailId, result.Cocktail);
            Assert.AreEqual(reviewDTO.AuthorId, result.UserId);
            Assert.AreEqual(reviewDTO.Comment, result.Comment);
            Assert.AreEqual(reviewDTO.Rating, result.Rating);
        }
    }
}
