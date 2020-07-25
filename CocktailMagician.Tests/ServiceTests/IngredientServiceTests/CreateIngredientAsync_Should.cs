using CocktailMagician.Data;
using CocktailMagician.Models;
using CocktailMagician.Services;
using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.Mappers.Contracts;
using CocktailMagician.Services.Providers.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace CocktailMagician.Tests.ServiceTests.IngredientServiceTests
{
    [TestClass]
    public class CreateIngredientAsync_Should
    {
        [TestMethod]
        public async Task CreateIngredient_WhenParamsAreValid()
        {
            //Arrange
            var mockDatetimeProvider = new Mock<IDateTimeProvider>();
            var mockMapper = new Mock<IIngredientMapper>();
            mockMapper.Setup(i => i.MapToIngredient(It.IsAny<IngredientDTO>()))
                .Returns<IngredientDTO>(i => new Ingredient
                {
                    Name = i.Name,
                    ImageData = i.ImageData,
                    ImageSource = i.ImageSource
                });
            var mockCocktailMapper = new Mock<ICocktailMapper>();
            var options = Utils.GetOptions(nameof(CreateIngredient_WhenParamsAreValid));

            var ingredientDTO = new IngredientDTO
            {
                Name = "Black Pepper",
                ImageData = new byte[] { },
                ImageSource = "~/Testimage/pqt-ozer-premium.jpg"
            };

            string imageBase64Data = Convert.ToBase64String(ingredientDTO.ImageData);
            ingredientDTO.ImageSource = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            
            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new IngredientService(mockDatetimeProvider.Object, mockMapper.Object,
                    mockCocktailMapper.Object, assertContext);
                var result = await sut.CreateIngredientAsync(ingredientDTO);

                Assert.IsInstanceOfType(result, typeof(IngredientDTO));
            }
        }
    }
}
