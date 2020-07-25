using CocktailMagician.Models;
using CocktailMagician.Services.DTOs;

namespace CocktailMagician.Services.Mappers.Contracts
{
    public interface IUserMapper
    {
        public UserDTO MapToUserDTO(User user);
        public User MapToUser(UserDTO userDTO);
    }
}
