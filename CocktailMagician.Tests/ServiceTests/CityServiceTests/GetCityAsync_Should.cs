using CocktailMagician.Data;
using CocktailMagician.Models;
using CocktailMagician.Services;
using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.Mappers.Contracts;
using CocktailMagician.Services.Providers.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CocktailMagician.Tests.ServiceTests.CityServiceTests
{
    [TestClass]
    public class GetCityAsync_Should
    {
        [TestMethod]
        public async Task ReturnNull_CityDoesNotExist()
        {
            //Arrange
            var mockIDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockICityMapper = new Mock<ICityMapper>();
            var mockIBarMapper = new Mock<IBarMapper>();

            var options = Utils.GetOptions(nameof(ReturnNull_CityDoesNotExist));

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new CityService(mockIDateTimeProvider.Object, assertContext, mockICityMapper.Object, mockIBarMapper.Object);

                var result = await sut.GetCityAsync(3);

                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public async Task Return_WhenParamsAreValid()
        {
            //Arrange
            var mockIDateTimeProvider = new Mock<IDateTimeProvider>();

            var mockIBarMapper = new Mock<IBarMapper>();
            mockIBarMapper
                .Setup(x => x.MapToBarDTO(It.IsAny<Bar>()))
                .Returns<Bar>(b => new BarDTO
                {
                    Id = b.Id,
                    Name = b.Name,
                    CityId = b.CityId,
                    Address = b.Address,
                    Phone = b.Phone,
                    AverageRating = b.AverageRating,
                });

            var mockICityMapper = new Mock<ICityMapper>();
            mockICityMapper
                .Setup(x => x.MapToCityDTO(It.IsAny<City>()))
                .Returns<City>(c => new CityDTO
                {
                    Id = c.Id,
                    Name = c.Name
                });

            var options = Utils.GetOptions(nameof(Return_WhenParamsAreValid));

            Utils.GetInMemoryDataBase(options);

            var expected = new City
            {
                Id = 2,
                Name = "Varna",
                Bars = new List<Bar>
                    {
                        new Bar
                        {
                            Id = 3,
                            Name = "The Beach",
                            CityId = 2,
                            Address = "Obikolna str.",
                            Phone = "0888 777 444",
                            AverageRating = 4.5,
                        }
                    },
                IsDeleted = false
            };


            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new CityService(mockIDateTimeProvider.Object, assertContext, mockICityMapper.Object, mockIBarMapper.Object);

                var result = await sut.GetCityAsync(2);

                Assert.AreEqual(expected.Id, result.Id);
                Assert.AreEqual(expected.Name, result.Name);
                Assert.AreEqual(expected.Bars.Count, result.Bars.Count);
            }
        }
    }
}
