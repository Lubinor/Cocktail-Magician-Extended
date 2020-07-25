using CocktailMagician.Models;
using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CocktailMagician.Tests.ServiceTests.DTOMappersTests
{
    [TestClass]
    public class BarReviewMapper_Should
    {
        [TestMethod]
        public void CorrectReturnInstanceType_ToBarReviewDTO()
        {
            //Arrange
            var sut = new BarReviewMapper();

            var bar = new Bar { Id = 1, Name = "Lorka" };
            var user = new User { Id = 2, UserName = "Tom" };
            var review = new BarsUsersReviews 
            { 
                BarId = 1, 
                Bar = bar, 
                UserId = 2, 
                User = user, 
                Comment = "Top!", 
                Rating = 4 
            };

            //Act
            var result = sut.MapToBarReviewDTO(review);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BarReviewDTO));
        }

        [TestMethod]
        public void CorrectMapping_ToBarReviewDTO()
        {
            //Arrange
            var sut = new BarReviewMapper();

            var options = Utils.GetOptions(nameof(CorrectReturnInstanceType_ToBarReviewDTO));
            var bar = new Bar { Id = 1, Name = "Lorka" };
            var user = new User { Id = 2, UserName = "Tom" };
            var review = new BarsUsersReviews
            {
                BarId = 1,
                Bar = bar,
                UserId = 2,
                User = user,
                Comment = "Top!",
                Rating = 4
            };

            //Act
            var result = sut.MapToBarReviewDTO(review);

            //Assert
            Assert.AreEqual(review.BarId, result.BarId);
            Assert.AreEqual(review.UserId, result.AuthorId);
            Assert.AreEqual(review.Bar.Name, result.BarName);
            Assert.AreEqual(review.User.UserName, result.Author);
            Assert.AreEqual(review.Comment, result.Comment);
            Assert.AreEqual(review.Rating, result.Rating);
        }

        [TestMethod]
        public void CorrectReturnInstanceType_ToBarReview()
        {
            //Arrange
            var sut = new BarReviewMapper();

            var options = Utils.GetOptions(nameof(CorrectReturnInstanceType_ToBarReview));

            var review = new BarReviewDTO
            {
                BarId = 1,
                AuthorId = 2,
                Comment = "Top!",
                Rating = 4,
            };

            //Act
            var result = sut.MapToBarReview(review);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BarsUsersReviews));
        }

        [TestMethod]
        public void CorrectMapping_ToBarReview()
        {
            //Arrange
            var sut = new BarReviewMapper();

            var options = Utils.GetOptions(nameof(CorrectMapping_ToBarReview));

            var review = new BarReviewDTO
            {
                BarId = 1,
                AuthorId = 2,
                Comment = "Top!",
                Rating = 4,
            };

            //Act
            var result = sut.MapToBarReview(review);

            //Assert
            Assert.AreEqual(review.BarId, result.BarId);
            Assert.AreEqual(review.AuthorId, result.UserId);
            Assert.AreEqual(review.Comment, result.Comment);
            Assert.AreEqual(review.Rating, result.Rating);
        }
    }
}
