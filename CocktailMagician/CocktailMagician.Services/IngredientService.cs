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
using System.Net;
using System.Threading.Tasks;

namespace CocktailMagician.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly IDateTimeProvider datetimeProvider;
        private readonly IIngredientMapper mapper;
        private readonly ICocktailMapper cocktailMapper;
        private readonly CocktailMagicianContext context;

        public IngredientService(IDateTimeProvider datetimeProvider, IIngredientMapper mapper,
            ICocktailMapper cocktailMapper, CocktailMagicianContext context)
        {
            this.datetimeProvider = datetimeProvider ?? throw new ArgumentNullException(nameof(datetimeProvider));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.cocktailMapper = cocktailMapper ?? throw new ArgumentNullException(nameof(cocktailMapper));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }
        /// <summary>
        /// Shows all available ingredients.
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<IngredientDTO>> GetAllIngredientsAsync()
        {
            var ingredients = this.context.Ingredients
                .Include(ingredient => ingredient.IngredientsCocktails)
                .ThenInclude(ic => ic.Cocktail)
                .Where(ingredient => ingredient.IsDeleted == false);

            var ingredientDTOs = await ingredients.Select(ingredient => mapper.MapToIngredientDTO(ingredient)).ToListAsync();

            return ingredientDTOs;

        }
        /// <summary>
        /// If id exist and the ingredient is not deleted, shows the ingredient with that id, otherwise returns null.
        /// </summary>
        /// <param name="id">The id of searched ingredient</param>
        /// <returns></returns>
        public async Task<IngredientDTO> GetIngredientAsync(int id)
        {
            var ingredient = await this.context.Ingredients
                .Include(ingredient => ingredient.IngredientsCocktails)
                .ThenInclude(ic => ic.Cocktail)
                .FirstOrDefaultAsync(ingredient => ingredient.Id == id & ingredient.IsDeleted == false);

            if (ingredient == null)
            {
                return null;
            }
            
            var ingredientDTO = mapper.MapToIngredientDTO(ingredient);
            var ingredientCocktails = ingredient.IngredientsCocktails.Select(x => x.Cocktail);
            ingredientDTO.CocktailDTOs = ingredientCocktails.Where(c => c.IsDeleted == false)
                .Select(x => cocktailMapper.MapToCocktailDTO(x)).ToList();

            return ingredientDTO;
        }
        /// <summary>
        /// Create new ingredient entity in the database.
        /// </summary>
        /// <param name="ingredientDTO"></param>
        /// <returns></returns>
        public async Task<IngredientDTO> CreateIngredientAsync(IngredientDTO ingredientDTO)
        {

            var ingredient = await this.context.Ingredients.FirstOrDefaultAsync(i => i.Name == ingredientDTO.Name);
           
            if (ingredient == null)
            {
                ingredient = mapper.MapToIngredient(ingredientDTO);
                ingredient.CreatedOn = datetimeProvider.GetDateTime();

                string imageBase64Data = Convert.ToBase64String(ingredient.ImageData);
                ingredient.ImageSource = string.Format("data:image/jpg;base64,{0}", imageBase64Data);

                this.context.Ingredients.Add(ingredient);
            }
            else if (ingredient.IsDeleted == true)
            {
                ingredient.Name = ingredientDTO.Name;
                ingredient.IsDeleted = false;
                //string imageBase64Data = Convert.ToBase64String(ingredient.ImageData);
                //ingredient.ImageSource = string.Format("data:image/jpg;base64,{0}", imageBase64Data);

                //this.context.Ingredients.Update(ingredient);
            }
            else
            {
                return null;
            }

            await this.context.SaveChangesAsync();
            
            return ingredientDTO;
        }
        /// <summary>
        /// If id exist and the ingredient is not deleted, update the ingredient with current info from ingredientDTO
        /// </summary>
        /// <param name="id">The id of the ingredient to be updated</param>
        /// <param name="ingredientDTO"></param>
        /// <returns></returns>
        public async Task<IngredientDTO> UpdateIngredientAsync(int id, IngredientDTO ingredientDTO)
        {
            var ingredient = await this.context.Ingredients
                .FirstOrDefaultAsync(ingredient => ingredient.Id == id & ingredient.IsDeleted == false);

            if (ingredient == null)
            {
                return null;
            }

            ingredient.Name = ingredientDTO.Name;

            if (ingredientDTO.ImageData != null)
            {
            ingredient.ImageData = ingredientDTO.ImageData;
            string imageBase64Data = Convert.ToBase64String(ingredient.ImageData);
            ingredient.ImageSource = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            }

            this.context.Ingredients.Update(ingredient);
            await this.context.SaveChangesAsync();

            return ingredientDTO;
        }
        /// <summary>
        /// If id exist and the ingredient is not deleted or not used in any cocktail, set the property "IsDeleted" to true and makes it
        /// not visible for users. Otherwise ingredient can not be deleted.
        /// </summary>
        /// <param name="id">The id of the ingredient to be deleted</param>
        /// <returns></returns>
        public async Task<bool> DeleteIngredientAsync(int id)
        {
            var ingredient = await this.context.Ingredients
                .Include(ingredient => ingredient.IngredientsCocktails)
                .ThenInclude(ic => ic.Cocktail)
                .FirstOrDefaultAsync(ingredient => ingredient.Id == id & ingredient.IsDeleted == false);

            if (ingredient == null)
            {
                return false;
            }

            if (ingredient.IngredientsCocktails.Any(c => c.IngredientId == id))
            {
                throw new ArgumentException("Ingredient still in use");
            }
            else
            {
                ingredient.IsDeleted = true;
                await this.context.SaveChangesAsync();
                return true;
            }
        }
        /// <summary>
        /// Shows collection of cocktails that matches certain criterias
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="pageSize">Count of the item shown on page</param>
        /// <param name="searchValue">Shows item/s that contain/s in its/their name/s the searced value</param>
        /// <param name="orderBy">The column by which the table wil be sorted</param>
        /// <param name="orderDirection">Ascending or descending</param>
        /// <returns></returns>
        public async Task<IList<IngredientDTO>> ListAllIngredientsAsync(int skip, int pageSize, string searchValue,
            string orderBy, string orderDirection)
        {
            var ingredients = this.context.Ingredients
                .Include(ingredient => ingredient.IngredientsCocktails)
                .ThenInclude(ic => ic.Cocktail)
                .Where(ingredient => ingredient.IsDeleted == false);

            if (!String.IsNullOrEmpty(orderBy))
            {
                if (String.IsNullOrEmpty(orderDirection) || orderDirection == "asc")
                {
                    ingredients = ingredients.OrderBy(orderBy);
                }
                else
                {
                    ingredients = ingredients.OrderByDescending(orderBy);
                }
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.ToLower();

                ingredients = ingredients
                     .Where(ingredient => ingredient.Name.ToLower()
                     .Contains(searchValue.ToLower()));
            }

            ingredients = ingredients
                .Skip(skip)
                .Take(pageSize);

            var ingredientDTOs = await ingredients.Select(ingredient => mapper.MapToIngredientDTO(ingredient)).ToListAsync();

            return ingredientDTOs;
        }
        /// <summary>
        /// Returns the count of all not deleted ingredients
        /// </summary>
        /// <returns></returns>
        public int GetAllIngredientsCount()
        {
            return this.context.Ingredients.Where(ingredient => ingredient.IsDeleted == false).Count();
        }
        /// <summary>
        /// Returns the count of all ingredients which names contains searchValue. If searchValue is null or
        /// empty string - returns the count of all not deleted ingredients
        /// </summary>
        /// <param name="searchValue">char or string that is contained by ingredient name</param>
        /// <returns></returns>
        public int GetAllFilteredIngredientsCount(string searchValue)
        {

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.ToLower();

                var ingredients = this.context.Ingredients
                     .Where(ingredient => ingredient.Name.ToLower().Contains(searchValue));
                return ingredients.Count();
            }
            return this.context.Ingredients.Where(ingredient => ingredient.IsDeleted == false).Count();
        }
        /// <summary>
        /// Method which checks if the passed "DTO" can be transormed to database model
        /// </summary>
        /// <param name="ingredientDTO">the object to be transormed to database model</param>
        /// <returns></returns>
        public ValidationModel ValidateIngredient(IngredientDTO ingredientDTO)
        {
            var validationModel = new ValidationModel();

            if (ingredientDTO == null)
            {
                validationModel.HasProperInputData = false;
            }
            if (ingredientDTO.Name == string.Empty || 
                ingredientDTO.Name.Any(x => !char.IsLetter(x) && !char.IsWhiteSpace(x)))
            {
                validationModel.HasValidName = false;
            }
            if (ingredientDTO.Name.Length < 2 || ingredientDTO.Name.Length > 30)
            {
                validationModel.HasProperNameLength = false;
            }
            return validationModel;
        }

        public bool IngredientIsUnique(IngredientDTO ingredientDTO)
        {
            if (this.context.Ingredients.Any(x => x.Name.ToLower().Equals(ingredientDTO.Name.ToLower())))
            {
                return false;
            }
            return true;
        }
    }
}
