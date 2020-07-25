using System.Collections.Generic;

namespace CocktailMagician.Services.DTOs
{
    public class UserDTO
    {
        public UserDTO()
        {

        }
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public ICollection<CocktailDTO> CreatedCocktails { get; set; } = new HashSet<CocktailDTO>();

    }
}
