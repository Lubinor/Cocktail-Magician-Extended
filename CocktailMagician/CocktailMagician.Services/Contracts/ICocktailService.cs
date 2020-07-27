using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.ValidationModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CocktailMagician.Services.Contracts
{
    public interface ICocktailService
    {
        Task<ICollection<CocktailDTO>> GetAllCocktailssAsync();
        Task<CocktailDTO> GetCocktailAsync(int id);
        Task<CocktailDTO> CreateCocktailAsync(CocktailDTO cocktailDTO);
        Task<CocktailDTO> UpdateCocktailAsync(int id, CocktailDTO cocktailDTO);
        Task<bool> DeleteCocktailAsync(int id);
        Task<IEnumerable<CocktailDTO>> ListAllCocktailsAsync(
            int skip, 
            int pageSize, 
            string searchValue,
            string orderBy, 
            string orderDirection);
        int GetAllCocktailsCount();
        int GetAllFilteredCocktailsCount(string searchValue);
        ValidationModel ValidateCocktail(CocktailDTO cocktailDTO);
        bool CocktailIsUnique(CocktailDTO cocktailDTO);
    }
}
