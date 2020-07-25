using CocktailMagician.Data;
using CocktailMagician.Models;
using CocktailMagician.Services;
using CocktailMagician.Services.Contracts;
using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.Mappers;
using CocktailMagician.Services.Mappers.Contracts;
using CocktailMagician.Services.Providers.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace CocktailMagician.Tests.ServiceTests.CocktailServiceTests
{
    [TestClass]
    public class CreateCocktailAsync_Should
    {
        [TestMethod]
        public async Task CreateCocktail_WhenParamsAreValid()
        {
            //Arrange
            var mockDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockMapper = new Mock<ICocktailMapper>();
            mockMapper.Setup(c => c.MapToCocktail(It.IsAny<CocktailDTO>()))
                .Returns<CocktailDTO>(c => new Cocktail
                {
                    Name = c.Name,
                    ImageData = c.ImageData,
                    ImageSource = c.ImageSource
                });
            var mockIngMapper = new Mock<IngredientMapper>();
            var mockBarMapper = new Mock<IBarMapper>();
            var mockCocktailReviewService = new Mock<ICocktailReviewService>();
            var options = Utils.GetOptions(nameof(CreateCocktail_WhenParamsAreValid));

            var cocktailDTO = new CocktailDTO
            {
                Name = "New Cocktail",
                ImageData = new byte[] {},
                ImageSource = "~/Testimage/pqt-ozer-premium.jpg"
            }; 
            
            string imageBase64Data = Convert.ToBase64String(cocktailDTO.ImageData);
            cocktailDTO.ImageSource = string.Format("data:image/jpg;base64,{0}", imageBase64Data);

            Utils.GetInMemoryDataBase(options);
            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new CocktailService(mockDateTimeProvider.Object, mockMapper.Object,
                    mockIngMapper.Object, mockBarMapper.Object, assertContext, mockCocktailReviewService.Object);
                var result = await sut.CreateCocktailAsync(cocktailDTO);

                Assert.IsInstanceOfType(result, typeof(CocktailDTO));
            }
        }
    }
}
