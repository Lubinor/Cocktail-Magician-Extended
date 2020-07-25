using CocktailMagician.Data;
using CocktailMagician.Services;
using CocktailMagician.Services.Mappers.Contracts;
using CocktailMagician.Services.Providers.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace CocktailMagician.Tests.ServiceTests.CityServiceTests
{
    [TestClass]
    public class DeleteCityAsync_Should
    {
        [TestMethod]
        public async Task ReturnFalse_CityDoesNotExistOrDeleted()
        {
            //Arrange
            var mockIDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockICityMapper = new Mock<ICityMapper>();
            var mockIBarMapper = new Mock<IBarMapper>();

            var options = Utils.GetOptions(nameof(ReturnFalse_CityDoesNotExistOrDeleted));

            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new CityService(mockIDateTimeProvider.Object, assertContext, mockICityMapper.Object, mockIBarMapper.Object);

                var result = await sut.DeleteCityAsync(4);

                Assert.IsFalse(result);
            }
        }

        [TestMethod]
        public async Task ReturnTrue_WhenDeletedSuccessfully()
        {
            //Arrange
            var mockIDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockICityMapper = new Mock<ICityMapper>();
            var mockIBarMapper = new Mock<IBarMapper>();

            var options = Utils.GetOptions(nameof(ReturnTrue_WhenDeletedSuccessfully));

            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new CityService(mockIDateTimeProvider.Object, assertContext, mockICityMapper.Object, mockIBarMapper.Object);

                var result = await sut.DeleteCityAsync(1);
                var deletedCity = await assertContext.Cities.FirstOrDefaultAsync(c => c.Id == 1);

                Assert.IsTrue(result);
                Assert.AreEqual(true, deletedCity.IsDeleted);
            }
        }
    }
}
