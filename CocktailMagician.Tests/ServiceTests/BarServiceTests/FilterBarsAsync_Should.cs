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

namespace CocktailMagician.Tests.ServiceTests.BarServiceTests
{
    [TestClass]
    public class FilterBarsAsync_Should
    {
        [TestMethod]
        public async Task ReturnAllBars_IfFilterIsNull()
        {
            //Arrange
            var mockIDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockIBarReviewService = new Mock<IBarReviewService>();
            var mockIBarMapper = new Mock<IBarMapper>();
            mockIBarMapper
                .Setup(x => x.MapToBarDTO(It.IsAny<Bar>()))
                .Returns<Bar>(b => new BarDTO { Id = b.Id, Name = b.Name });
            var orderBy = "name";
            var direction = "asc";

            var options = Utils.GetOptions(nameof(ReturnAllBars_IfFilterIsNull));

            var expected = new List<BarDTO>
            {
                new BarDTO
                {
                    Id = 2,
                    Name= "Bilkova"
                },
                new BarDTO
                {
                    Id = 1,
                    Name= "Lorka"
                },
                new BarDTO
                {
                    Id = 3,
                    Name= "The Beach"
                },
            };

            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new BarService(mockIDateTimeProvider.Object, assertContext, mockIBarMapper.Object, mockIBarReviewService.Object);

                var result = await sut.ListAllBarsAsync(0,10,null,orderBy,direction);

                Assert.AreEqual(expected.Count, result.Count);
               
                for (int i = 0; i < expected.Count; i++)
                {
                    Assert.AreEqual(expected[i].Name, result[i].Name);
                }
            }
        }

        [TestMethod]
        public async Task ReturnEmpty_IfNoMatchesFound()
        {
            //Arrange
            var mockIDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockIBarReviewService = new Mock<IBarReviewService>();

            var mockIBarMapper = new Mock<IBarMapper>();
            mockIBarMapper
                .Setup(x => x.MapToBarDTO(It.IsAny<Bar>()))
                .Returns<Bar>(b => new BarDTO { Id = b.Id, Name = b.Name });
            var filter = "ZzzZ";
            var orderBy = "name";
            var direction = "asc";
            var options = Utils.GetOptions(nameof(ReturnEmpty_IfNoMatchesFound));

            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new BarService(mockIDateTimeProvider.Object, assertContext, mockIBarMapper.Object, mockIBarReviewService.Object);

                var result = await sut.ListAllBarsAsync(0,10,filter,orderBy,direction);

                Assert.AreEqual(0, result.Count);
            }
        }

        [TestMethod]
        public async Task ReturnValid_StringFilter()
        {
            //Arrange
            var mockIDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockIBarReviewService = new Mock<IBarReviewService>();

            var mockIBarMapper = new Mock<IBarMapper>();
            mockIBarMapper
                .Setup(x => x.MapToBarDTO(It.IsAny<Bar>()))
                .Returns<Bar>(b => new BarDTO { Id = b.Id, Name = b.Name });
            var filter = "lOr";
            var orderBy = "name";
            var direction = "asc";

            var options = Utils.GetOptions(nameof(ReturnValid_StringFilter));

            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new BarService(mockIDateTimeProvider.Object, assertContext, mockIBarMapper.Object, mockIBarReviewService.Object);

                var result = await sut.ListAllBarsAsync(0,10,filter,orderBy,direction);
                var expectedBar = result.FirstOrDefault(x => x.Name == "Lorka");

                Assert.AreEqual(1, result.Count);
                Assert.AreEqual("Lorka", expectedBar.Name);
            }
        }
    }
}