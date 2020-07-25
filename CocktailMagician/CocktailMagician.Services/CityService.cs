using CocktailMagician.Data;
using CocktailMagician.Services.Contracts;
using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.Helpers;
using CocktailMagician.Services.Mappers.Contracts;
using CocktailMagician.Services.Providers.Contracts;
using CocktailMagician.Services.ValidationModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace CocktailMagician.Services
{
    public class CityService : ICityService
    {
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly CocktailMagicianContext context;
        private readonly ICityMapper cityMapper;
        private readonly IBarMapper barMapper;

        public CityService(IDateTimeProvider dateTimeProvider, CocktailMagicianContext context, ICityMapper cityMapper, IBarMapper barMapper)
        {
            this.dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.cityMapper = cityMapper ?? throw new ArgumentNullException(nameof(cityMapper));
            this.barMapper = barMapper ?? throw new ArgumentNullException(nameof(barMapper));
        }

        public async Task<ICollection<CityDTO>> GetAllCitiesAsync()
        {
            var cities = this.context.Cities
                .Include(c => c.Bars)
                .Where(c => !c.IsDeleted);

            var cityDTOs = await cities
                .Select(c => this.cityMapper.MapToCityDTO(c))
                .ToListAsync();

            return cityDTOs;
        }

        public async Task<CityDTO> GetCityAsync(int id)
        {
            var city = await this.context.Cities
                .Include(c => c.Bars)
                .Where(c => !c.IsDeleted && c.Id == id)
                .FirstOrDefaultAsync();

            if (city == null)
            {
                return null;
            }

            var cityDTO = this.cityMapper.MapToCityDTO(city);

            cityDTO.Bars = city.Bars
                .Where(bar => !bar.IsDeleted)
                .Select(bar => barMapper.MapToBarDTO(bar))
                .ToList();

            return cityDTO;
        }

        public async Task<CityDTO> CreateCityAsync(CityDTO cityDTO)
        {
            if (cityDTO == null)
            {
                return null;
            }

            var city = cityMapper.MapToCity(cityDTO);
            city.CreatedOn = dateTimeProvider.GetDateTime();

            this.context.Cities.Add(city);
            await this.context.SaveChangesAsync();

            var newCityDTO = cityMapper.MapToCityDTO(city);

            return newCityDTO;
        }

        public async Task<CityDTO> UpdateCityAsync(int id, CityDTO cityDTO)
        {
            var city = await this.context.Cities
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if (city == null)
            {
                return null;
            }

            city.Name = cityDTO.Name; //update manually, instead of replacing the whole object, to avoid overwriting collections?

            this.context.Cities.Update(city);
            await this.context.SaveChangesAsync();

            var updatedCityDTO = cityMapper.MapToCityDTO(city);

            return updatedCityDTO;
        }

        public async Task<bool> DeleteCityAsync(int id)
        {
            var city = await this.context.Cities
                .Include(c => c.Bars)
                    .ThenInclude(b => b.BarReviews)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if (city == null)
            {
                return false;
            }

            city.IsDeleted = true;

            foreach (var bar in city.Bars)
            {
                bar.IsDeleted = true;
                this.context.Bars.Update(bar);

                foreach (var review in bar.BarReviews)
                {
                    review.IsDeleted = true;
                    this.context.BarsUsersReviews.Update(review);
                }
            }
            this.context.Cities.Update(city);
            await this.context.SaveChangesAsync();

            return true;
        }

        public async Task<IList<CityDTO>> ListAllCitiesAsync(int skip, int pageSize, string searchValue, string orderBy, string orderDirection)
        {
            var cities = this.context.Cities
                .Include(city => city.Bars)
                    .Where(city => !city.IsDeleted);

            if (!String.IsNullOrEmpty(orderBy))
            {
                if (String.IsNullOrEmpty(orderDirection) || orderDirection == "asc")
                {
                    cities = cities.OrderBy(orderBy);
                }
                else
                {
                    cities = cities.OrderByDescending(orderBy);
                }
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.ToLower();

                cities = cities
                     .Where(city => city.Name.ToLower()
                     .Contains(searchValue.ToLower()));
            }

            cities = cities
                .Skip(skip)
                .Take(pageSize);

            var cityDTOs = await cities.Select(city => cityMapper.MapToCityDTO(city)).ToListAsync();

            return cityDTOs;
        }

        public int GetAllCitiesCount()
        {
            return this.context.Cities.Where(city => !city.IsDeleted).Count();
        }

        public int GetAllFilteredCitiesCount(string searchValue)
        {
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.ToLower();

                var cities = this.context.Cities
                     .Where(city => city.Name.ToLower()
                     .Contains(searchValue.ToLower()));

                return cities.Count();
            }
            return this.context.Cities.Where(city => !city.IsDeleted).Count();
        }

        public ValidationModel ValidateCity(CityDTO cityDTO)
        {
            var validationModel = new ValidationModel();

            if (cityDTO == null)
            {
                validationModel.HasProperInputData = false;
            }
            if (cityDTO.Name == string.Empty || cityDTO.Name.Any(x => !char.IsLetter(x) && !char.IsWhiteSpace(x)))
            {
                validationModel.HasValidName = false;
            }
            if (cityDTO.Name.Length < 2 || cityDTO.Name.Length > 30)
            {
                validationModel.HasProperNameLength = false;
            }
            return validationModel;
        }
        public bool CityIsUnique(CityDTO cityDTO)
        {
            if (this.context.Cities.Any(x => x.Name.ToLower().Equals(cityDTO.Name.ToLower())))
            {
                return false;
            }
            return true;
        }
    }
}
