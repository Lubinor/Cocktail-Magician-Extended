using CocktailMagician.Data;
using CocktailMagician.Models;
using CocktailMagician.Services;
using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.Mappers;
using CocktailMagician.Services.Mappers.Contracts;
using CocktailMagician.Services.Providers.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace CocktailMagician.Tests.ServiceTests.IngredientServiceTests
{
    [TestClass]
    public class DeleteIngredient_Should
    {
        [TestMethod]
        public async Task DeleteIngredient_WhenConditionsAreMet()
        {
            //Arrange
            var mockDatetimeProvider = new Mock<IDateTimeProvider>();
            var mockMapper = new Mock<IIngredientMapper>();
            mockMapper.Setup(i => i.MapToIngredientDTO(It.IsAny<Ingredient>()))
                .Returns<Ingredient>(i => new IngredientDTO { Name = i.Name });
            var mockCocktailMapper = new Mock<CocktailMapper>();
            var options = Utils.GetOptions(nameof(DeleteIngredient_WhenConditionsAreMet));
            
            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new IngredientService(mockDatetimeProvider.Object, mockMapper.Object,
                    mockCocktailMapper.Object, assertContext);
                var result = await sut.DeleteIngredientAsync(5);

                Assert.IsTrue(result);
            }
        }
        [TestMethod]
        public async Task ReturnFalse_WhenIdNotFound()
        {
            //Arrange
            var mockDatetimeProvider = new Mock<IDateTimeProvider>();
            var mockMapper = new Mock<IIngredientMapper>();
            mockMapper.Setup(i => i.MapToIngredientDTO(It.IsAny<Ingredient>()))
                .Returns<Ingredient>(i => new IngredientDTO { Name = i.Name });
            var mockCocktailMapper = new Mock<ICocktailMapper>();
            var options = Utils.GetOptions(nameof(ReturnFalse_WhenIdNotFound));
            
            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new IngredientService(mockDatetimeProvider.Object, mockMapper.Object,
                    mockCocktailMapper.Object, assertContext);
                var result = await sut.DeleteIngredientAsync(9);

                Assert.IsFalse(result);
            }
        }
        //[TestMethod]
        //public async Task Throw_WhenIngredientStillInUse()
        //{
        //    var mockDatetimeProvider = new Mock<IDateTimeProvider>();
        //    var mockMapper = new Mock<IIngredientMapper>();
        //    mockMapper.Setup(i => i.MapToIngredientDTO(It.IsAny<Ingredient>()))
        //        .Returns<Ingredient>(i => new IngredientDTO { Name = i.Name });
        //    var mockCocktailMapper = new Mock<ICocktailMapper>();
        //    mockCocktailMapper.Setup(c => c.MapToCocktailDTO(It.IsAny<Cocktail>()))
        //        .Returns<Cocktail>(c => new CocktailDTO { Name = c.Name });
        //    var options = Utils.GetOptions(nameof(Throw_WhenIngredientStillInUse));
        //    Utils.GetInMemoryDataBase(options);

        //    //Act & Assert
        //    using (var assertContext = new CocktailMagicianContext(options))
        //    {
        //        var sut = new IngredientService(mockDatetimeProvider.Object, mockMapper.Object,
        //            mockCocktailMapper.Object, assertContext);
        //        var result = await sut.DeleteIngredientAsync(2);

        //        Assert.ThrowsException<ArgumentException>(() => throw new ArgumentException());
        //        //() => throw new ArgumentException($"Ingredient still in use")
        //    }
        //}
    }
}
