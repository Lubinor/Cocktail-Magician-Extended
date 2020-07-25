using CocktailMagician.Models;
using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.Mappers.Contracts;
using System.Linq;

namespace CocktailMagician.Services.Mappers
{
    public class BarMapper : IBarMapper
    {
        private readonly ICocktailMapper cocktailMapper;

        public BarMapper(ICocktailMapper cocktailMapper)
        {
            this.cocktailMapper = cocktailMapper;
        }

        public BarMapper()
        {
        }

        public BarDTO MapToBarDTO(Bar bar)
        {
            BarDTO barDTO = new BarDTO();

            barDTO.Id = bar.Id;
            barDTO.Name = bar.Name;
            barDTO.CityId = bar.CityId;
            barDTO.CityName = bar.City.Name;
            barDTO.Address = bar.Address;
            barDTO.Phone = bar.Phone;
            barDTO.AverageRating = bar.AverageRating;

            var barCocktails = bar.BarCocktails
                                    .Select(b => b.Cocktail)
                                    .Where(c => !c.IsDeleted)
                                    .ToList();

            barDTO.Cocktails = barCocktails
                                    .Select(c => new CocktailDTO 
                                    {
                                        Id = c.Id,
                                        Name = c.Name,
                                        AverageRating = c.AverageRating
                                    })
                                    .ToList();

            barDTO.ImageData = bar.ImageData;
            barDTO.ImageSource = bar.ImageSource;

            return barDTO;
        }
        public Bar MapToBar(BarDTO barDTO)
        {
            Bar bar = new Bar
            {
                Name = barDTO.Name,
                CityId = barDTO.CityId,
                Address = barDTO.Address,
                Phone = barDTO.Phone,
                AverageRating = barDTO.AverageRating,
                ImageData = barDTO.ImageData,
            };

            //var barCocktails = barDTO.Cocktails
            //                        .Select(c => cocktailMapper.MapToCocktail(c))
            //                        .ToList();

            //bar.BarCocktails = barCocktails
            //                        .Select(x => new BarsCocktails { BarId = barDTO.Id, CocktailId = x.Id })
            //                        .ToList();

            return bar;
        }
    }
}
