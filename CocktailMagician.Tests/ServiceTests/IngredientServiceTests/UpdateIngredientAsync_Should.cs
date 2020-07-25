using CocktailMagician.Data;
using CocktailMagician.Services;
using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.Mappers;
using CocktailMagician.Services.Providers.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace CocktailMagician.Tests.ServiceTests.IngredientServiceTests
{
    [TestClass]
    public class UpdateIngredientAsync_Should
    {
        [TestMethod]
        public async Task UpdateIngredient_WhenParamsAreValid()
        {
            //Arrange
            var mockDateTiemProvider = new Mock<IDateTimeProvider>();
            var mockMapper = new Mock<IngredientMapper>();
            var mockCocktailMapper = new Mock<CocktailMapper>();
            var options = Utils.GetOptions(nameof(UpdateIngredient_WhenParamsAreValid));

            var expected = new IngredientDTO
            {
                Id = 2,
                Name = "Portocal Juice"
            };
            Utils.GetInMemoryDataBase(options);

            //Act & Assert

            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new IngredientService(mockDateTiemProvider.Object, mockMapper.Object,
                    mockCocktailMapper.Object, assertContext);
                var result = await sut.UpdateIngredientAsync(2, expected);

                Assert.AreEqual(expected.Name, result.Name);
            }
        }
        [TestMethod]
        public async Task ReturnNull_WhenIdNotFound()
        {
            //Arrange
            var mockDateTiemProvider = new Mock<IDateTimeProvider>();
            var mockMapper = new Mock<IngredientMapper>();
            var mockCocktailMapper = new Mock<CocktailMapper>();
            var options = Utils.GetOptions(nameof(ReturnNull_WhenIdNotFound));
            var newDTO = new IngredientDTO();

            Utils.GetInMemoryDataBase(options);

            //Act & Assert

            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new IngredientService(mockDateTiemProvider.Object, mockMapper.Object,
                    mockCocktailMapper.Object, assertContext);
                var result = await sut.UpdateIngredientAsync(6, newDTO);

                Assert.IsNull(result);
            }
        }

    }
}
