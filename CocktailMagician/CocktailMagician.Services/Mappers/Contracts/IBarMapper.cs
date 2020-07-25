using CocktailMagician.Models;
using CocktailMagician.Services.DTOs;

namespace CocktailMagician.Services.Mappers.Contracts
{
    public interface IBarMapper
    {
        public BarDTO MapToBarDTO(Bar bar);
        public Bar MapToBar(BarDTO barDTO);
    }
}
