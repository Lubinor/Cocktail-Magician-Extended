using CocktailMagician.Models;
using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.Mappers.Contracts;
using System.Linq;

namespace CocktailMagician.Services.Mappers
{
    public class UserMapper : IUserMapper
    {
        public UserDTO MapToUserDTO(User user)
        {
            var userDTO = new UserDTO 
            {
                Id = user.Id,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                CreatedCocktails = user.CreatedCocktails
                            .Where(c => !c.IsDeleted) //is it ok here?
                            .Select(cocktail => new CocktailDTO { Id = cocktail.Id, Name = cocktail.Name })
                            .ToList(),
            };

            return userDTO;
        }

        public User MapToUser(UserDTO userDTO)
        {
            var user = new User
            {
                Id = userDTO.Id,
                UserName = userDTO.UserName,
                PhoneNumber = userDTO.PhoneNumber,
                Email = userDTO.Email
            };

            return user;
        }
    }
}
