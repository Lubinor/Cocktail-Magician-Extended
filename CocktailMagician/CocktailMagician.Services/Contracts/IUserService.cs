using CocktailMagician.Services.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CocktailMagician.Services.Contracts
{
    public interface IUserService
    {
        Task<UserDTO> GetUserAsync(int id);
        Task<ICollection<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> CreateUserAsync(UserDTO userDTO);
        Task<UserDTO> UpdateUserAsync(int id, UserDTO userDTO);
        Task<bool> DeleteUserAsync(int id);
    }
}
