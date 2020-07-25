using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.ValidationModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CocktailMagician.Services.Contracts
{
    public interface IBarService
    {
        Task<ICollection<BarDTO>> GetAllBarsAsync(string sortMethod = "default");
        Task<BarDTO> GetBarAsync(int id);
        Task<BarDTO> CreateBarAsync(BarDTO barDTO);
        Task<BarDTO> UpdateBarAsync(int id, BarDTO barDTO);
        Task<bool> DeleteBarAsync(int id);
        Task<IList<BarDTO>> ListAllBarsAsync(int skip, int pageSize, string searchValue,
                                                         string orderBy, string odrderDirection);
        int GetAllBarsCount();
        int GetAllFilteredBarsCount(string searchValue);
        ValidationModel ValidateBar(BarDTO barDTO);
        bool BarIsUnique(BarDTO barDTO);
    }
}
