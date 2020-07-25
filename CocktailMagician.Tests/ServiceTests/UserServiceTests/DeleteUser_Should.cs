using CocktailMagician.Data;
using CocktailMagician.Models;
using CocktailMagician.Services;
using CocktailMagician.Services.Mappers.Contracts;
using CocktailMagician.Services.Providers.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace CocktailMagician.Tests.ServiceTests.UserServiceTests
{
    [TestClass]
    public class DeleteUser_Should
    {
        [TestMethod]
        public async Task ReturnFalse_IfUserMissingOrDeleted()
        {
            //Arrange
            var mockDatetimeProvider = new Mock<IDateTimeProvider>();
            var mockIUserMapper = new Mock<IUserMapper>();

            var options = Utils.GetOptions(nameof(ReturnFalse_IfUserMissingOrDeleted));

            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new UserService(mockDatetimeProvider.Object, assertContext, mockIUserMapper.Object);
                var result = await sut.DeleteUserAsync(3);

                Assert.IsFalse(result);
            }
        }

        [TestMethod]
        public async Task DeleteUser_WhenParamsAreValid()
        {
            //Arrange
            var mockDatetimeProvider = new Mock<IDateTimeProvider>();
            var mockIUserMapper = new Mock<IUserMapper>();

            var options = Utils.GetOptions(nameof(DeleteUser_WhenParamsAreValid));

            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new UserService(mockDatetimeProvider.Object, assertContext, mockIUserMapper.Object);
                var result = await sut.DeleteUserAsync(1);

                var deletedUser = await assertContext.Users.FirstOrDefaultAsync(u => u.Id == 1);

                Assert.IsTrue(result);
                Assert.AreEqual(true, deletedUser.IsDeleted);
            }
        }
    }
}
