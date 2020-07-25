using CocktailMagician.Data;
using CocktailMagician.Models;
using CocktailMagician.Services;
using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.Mappers.Contracts;
using CocktailMagician.Services.Providers.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace CocktailMagician.Tests.ServiceTests.UserServiceTests
{
    [TestClass]
    public class GetAllUsers_Should
    {
        [TestMethod]
        public async Task ReturnEmpty_IfNoUsers()
        {
            //Arrange
            var mockDatetimeProvider = new Mock<IDateTimeProvider>();
            var mockIUserMapper = new Mock<IUserMapper>();

            var options = Utils.GetOptions(nameof(ReturnEmpty_IfNoUsers));

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new UserService(mockDatetimeProvider.Object, assertContext, mockIUserMapper.Object);
                var result = await sut.GetAllUsersAsync();

                Assert.AreEqual(0, assertContext.Users.Count());
            }
        }

        [TestMethod]
        public async Task Return_ProperUserCount()
        {
            //Arrange
            var mockDatetimeProvider = new Mock<IDateTimeProvider>();
            var mockIUserMapper = new Mock<IUserMapper>();
            mockIUserMapper
                .Setup(x => x.MapToUserDTO(It.IsAny<User>()))
                .Returns<User>(x => new UserDTO { UserName = x.UserName, Email = x.Email, PhoneNumber = x.PhoneNumber });

            var options = Utils.GetOptions(nameof(Return_ProperUserCount));

            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new UserService(mockDatetimeProvider.Object, assertContext, mockIUserMapper.Object);
                var result = await sut.GetAllUsersAsync();

                Assert.AreEqual(2, assertContext.Users.Count());
            }
        }
    }
}
