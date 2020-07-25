//using CocktailMagician.Data;
//using CocktailMagician.Models;
//using CocktailMagician.Services;
//using CocktailMagician.Services.Contracts;
//using CocktailMagician.Services.DTOs;
//using CocktailMagician.Services.Mappers;
//using CocktailMagician.Services.Mappers.Contracts;
//using CocktailMagician.Services.Providers.Contracts;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace CocktailMagician.Tests.ServiceTests.CocktailServiceTests
//{
//    [TestClass]
//    public class GetCocktailAsync_Should
//    {
//        [TestMethod]
//        public async Task ReturnCocktail_WhenFound()
//        {
//            //Arrange
//            var mockDateTimeProvider = new Mock<IDateTimeProvider>();
//            var mockMapper = new Mock<ICocktailMapper>(); 
//            //mockMapper.Setup(c => c.MapToCocktailDTO(It.IsAny<Cocktail>()))
//            //    .Returns<Cocktail>(c => new CocktailDTO
//            //    {
//            //        Id = c.Id,
//            //        Name = c.Name,
//            //        AverageRating = c.AverageRating,
//            //        IsDeleted = c.IsDeleted,
//            //    });
//            var mockIngredientMapper = new Mock<IngredientMapper>();
//            var mockBarMapper = new Mock<IBarMapper>();
//            var mockCocktailReviewService = new Mock<ICocktailReviewService>();
//            var options = Utils.GetOptions(nameof(ReturnCocktail_WhenFound));
//            var expected = new CocktailDTO
//            {
//                Id = 2,
//                Name = "Gin Fizz",
//                AverageRating = 4.9,
//                IsDeleted = false,
//                Ingredients = new List<IngredientDTO>
//                {
//                    new IngredientDTO
//                    {
//                        Id = 3,
//                        Name = "Dry Gin"
//                    },
//                    new IngredientDTO
//                    {
//                        Id = 4,
//                        Name = "Tonic",
//                    },
//                }
//            };

//            Utils.GetInMemoryDataBase(options);

//            //Act & Assert
//            using (var assertContext = new CocktailMagicianContext(options))
//            {
//                var sut = new CocktailService(mockDateTimeProvider.Object, mockMapper.Object,
//                    mockIngredientMapper.Object, mockBarMapper.Object, assertContext, mockCocktailReviewService.Object);
//                var result = await sut.GetCocktailAsync(2);

//                Assert.AreEqual(expected.Id, result.Id);
//                Assert.AreEqual(expected.Name, result.Name);
//                Assert.AreEqual(expected.AverageRating, result.AverageRating);
//                Assert.AreEqual(expected.Ingredients.ToList().Count, result.Ingredients.ToList().Count);
//            }
//        }
//    }
//}
