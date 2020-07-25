using CocktailMagician.Data;
using CocktailMagician.Models;
using CocktailMagician.Services;
using CocktailMagician.Services.Contracts;
using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.Mappers.Contracts;
using CocktailMagician.Services.Providers.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Tests.ServiceTests.CocktailServiceTests
{
    [TestClass]
    public class UpdateCocktail_Should
    {
        [TestMethod]
        public async Task UpdateCocktail_WhenParamsAreValid()
        {
            //Arrange
            var mockDateTimeprovider = new Mock<IDateTimeProvider>();
            var mockCocktailMapper = new Mock<ICocktailMapper>();
            mockCocktailMapper.Setup(c => c.MapToCocktail(It.IsAny<CocktailDTO>()))
                .Returns<CocktailDTO>(c => new Cocktail { Id = c.Id, Name = c.Name, IsDeleted = c.IsDeleted});
            var mockIngMapper = new Mock<IIngredientMapper>();
            mockIngMapper.Setup(i => i.MapToIngredient(It.IsAny<IngredientDTO>()))
                .Returns<IngredientDTO>(i => new Ingredient { Id = i.Id, Name = i.Name });
            var mockBarMapper = new Mock<IBarMapper>();
            var mockCocktailReviewService = new Mock<ICocktailReviewService>();
            var options = Utils.GetOptions(nameof(UpdateCocktail_WhenParamsAreValid));
            
            Utils.GetInMemoryDataBase(options);
            
            var newDTO = new CocktailDTO
            {
                
                Name = "Gin sTonik",
                Ingredients = new List<IngredientDTO>
                {
                    new IngredientDTO
                    {
                        Id = 3
                    },
                    new IngredientDTO
                    {
                        Id = 4
                    }
                }
            };

            //Act & Assert

            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new CocktailService(mockDateTimeprovider.Object, mockCocktailMapper.Object,
                   mockIngMapper.Object, mockBarMapper.Object, assertContext, mockCocktailReviewService.Object);
                var result = await sut.UpdateCocktailAsync(2, newDTO);

                Assert.AreEqual(newDTO.Id, result.Id);
                Assert.AreEqual(newDTO.Name, result.Name);
                Assert.AreEqual(newDTO.IsDeleted, result.IsDeleted);
                Assert.AreEqual(newDTO.Ingredients.ToList().Count, result.Ingredients.ToList().Count);
                CollectionAssert.AreEqual(newDTO.Ingredients.ToList(), result.Ingredients.ToList());
            }
        }
    }
}
 