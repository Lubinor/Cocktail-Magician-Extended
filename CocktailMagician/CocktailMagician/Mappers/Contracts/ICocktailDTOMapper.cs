using CocktailMagician.Services.DTOs;
using CocktailMagician.Web.Models;

namespace CocktailMagician.Web.Mappers.Contracts
{
    public interface ICocktailDTOMapper
    {
        public CocktailViewModel MapToVMFromDTO(CocktailDTO cocktailDTO);
        public CocktailDTO MapToDTOFromVM(CocktailViewModel cocktailVM);
        public CocktailDTO MapToDTOFromVM(CreateCocktailViewModel createCocktailVM);
        public CocktailDTO MapToDTOFromVM(EditCocktailViewModel editCocktailVM);

    }
}
