using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CocktailMagician.Services.Contracts;
using CocktailMagician.Web.Mappers.Contracts;
using CocktailMagician.Web.Models;
using System.Linq.Dynamic;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using NToastNotify;
using CocktailMagician.Web.Utilities;
using Ganss.XSS;

namespace CocktailMagician.Web.Controllers
{
    public class IngredientsController : Controller
    {
        private readonly IIngredientService ingredientService;
        private readonly ICocktailService cocktailService;
        private readonly IIngredientDTOMapper ingredientDTOMapper;
        private readonly ICocktailDTOMapper cocktailDTOMapper;
        private readonly IToastNotification toaster;

        public IngredientsController(
            IIngredientService ingredientService,
            ICocktailService cocktailService,
            IIngredientDTOMapper ingredientDTOMapper,
            ICocktailDTOMapper cocktailDTOMapper,
            IToastNotification toaster)

        {
            this.ingredientService = ingredientService ?? throw new ArgumentNullException(nameof(ingredientService));
            this.cocktailService = cocktailService ?? throw new ArgumentNullException(nameof(cocktailService));
            this.ingredientDTOMapper = ingredientDTOMapper ?? throw new ArgumentNullException(nameof(ingredientDTOMapper));
            this.cocktailDTOMapper = cocktailDTOMapper ?? throw new ArgumentNullException(nameof(cocktailDTOMapper));
            this.toaster = toaster ?? throw new ArgumentNullException(nameof(toaster));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            try
            {
                var ingredientDTOs = await this.ingredientService.GetAllIngredientsAsync();
                var ingredientVMs = ingredientDTOs.Select(ingredientDTO =>
                    ingredientDTOMapper.MapToVMFromDTO(ingredientDTO)).ToList();

                foreach (var ingredient in ingredientVMs)
                {
                    ingredient.Cocktails = (await this.cocktailService.GetAllCocktailssAsync())
                        .Where(cocktail => cocktail.Ingredients.Any(x => x.Name == ingredient.Name))
                        .Select(cdto => cocktailDTOMapper.MapToVMFromDTO(cdto)).ToList();
                }

                return View(ingredientVMs);
            }
            catch (Exception)
            {
                this.toaster.AddWarningToastMessage(ToastrConsts.GenericError);
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var ingredientDTO = await this.ingredientService.GetIngredientAsync(id);

                if (ingredientDTO == null)
                {
                    this.toaster.AddWarningToastMessage(ToastrConsts.NotFound);
                    return RedirectToAction(nameof(Index));
                }

                var ingredientVM = this.ingredientDTOMapper.MapToVMFromDTO(ingredientDTO);

                return View(ingredientVM);
            }
            catch (Exception)
            {
                this.toaster.AddWarningToastMessage(ToastrConsts.GenericError);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        [Authorize(Roles = "Cocktail Magician")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cocktail Magician")]
        public async Task<IActionResult> Create(IngredientViewModel ingredientVM)
        {
            //string ingredientName = HttpUtility.HtmlEncode(ingredientVM.Name);
            //ingredientVM.Name = ingredientName;

            var sanitizer = new HtmlSanitizer();
            ingredientVM.Name = sanitizer.Sanitize(ingredientVM.Name);

            if (ingredientVM.File == null)
            {
                this.toaster.AddWarningToastMessage(ToastrConsts.NoPicture);
                return RedirectToAction(nameof(Create));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (ingredientVM.File.Length > 0)
                    {
                        MemoryStream ms = new MemoryStream();
                        await ingredientVM.File.CopyToAsync(ms);
                        ingredientVM.ImageData = ms.ToArray();

                        ms.Close();
                        ms.Dispose();
                    }
                    var ingredientDTO = this.ingredientDTOMapper.MapToDTOFromVM(ingredientVM);

                    var validationResult = this.ingredientService.ValidateIngredient(ingredientDTO);

                    if (!validationResult.HasProperInputData)
                    {
                        this.toaster.AddWarningToastMessage(ToastrConsts.NullModel);
                        return RedirectToAction(nameof(Create));
                    }
                    if (!validationResult.HasProperNameLength)
                    {
                        this.toaster.AddWarningToastMessage(ToastrConsts.WrongNameLength);
                        return RedirectToAction(nameof(Create));
                    }
                    if (!validationResult.HasValidName)
                    {
                        this.toaster.AddWarningToastMessage(ToastrConsts.NameNotValid);
                        return RedirectToAction(nameof(Create));
                    }

                    bool isUnique = this.ingredientService.IngredientIsUnique(ingredientDTO);

                    if (!isUnique)
                    {
                        this.toaster.AddWarningToastMessage(ToastrConsts.NotUnique);
                        return RedirectToAction(nameof(Create));
                    }

                    await this.ingredientService.CreateIngredientAsync(ingredientDTO);

                    this.toaster.AddSuccessToastMessage(ToastrConsts.Success);
                    return RedirectToAction(nameof(Index));
                }

                catch (Exception)
                {
                    this.toaster.AddWarningToastMessage(ToastrConsts.GenericError);
                    return RedirectToAction(nameof(Index));
                }
            }
            this.toaster.AddWarningToastMessage(ToastrConsts.IncorrectInput);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Cocktail Magician")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var ingredientDTO = await this.ingredientService.GetIngredientAsync(id);

                if (ingredientDTO == null)
                {
                    this.toaster.AddWarningToastMessage(ToastrConsts.NotFound);
                    return RedirectToAction(nameof(Index));
                }

                var editIngredientVM = new EditIngredientViewModel
                {
                    Id = ingredientDTO.Id,
                    Name = ingredientDTO.Name
                };

                return View(editIngredientVM);
            }
            catch (Exception)
            {
                this.toaster.AddWarningToastMessage(ToastrConsts.GenericError);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cocktail Magician")]
        public async Task<IActionResult> Edit(int id, EditIngredientViewModel editIngredientVM)
        {
            var sanitizer = new HtmlSanitizer();
            editIngredientVM.Name = sanitizer.Sanitize(editIngredientVM.Name);

            if (id != editIngredientVM.Id)
            {
                this.toaster.AddWarningToastMessage(ToastrConsts.GenericError);
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (editIngredientVM.File != null && editIngredientVM.File.Length > 0)
                    {
                        MemoryStream ms = new MemoryStream();
                        await editIngredientVM.File.CopyToAsync(ms);
                        editIngredientVM.ImageData = ms.ToArray();

                        ms.Close();
                        ms.Dispose();
                    }

                    var ingredientDTO = this.ingredientDTOMapper.MapToDTOFromVM(editIngredientVM);

                    var validationResult = this.ingredientService.ValidateIngredient(ingredientDTO);

                    if (!validationResult.HasProperInputData)
                    {
                        this.toaster.AddWarningToastMessage(ToastrConsts.NullModel);
                        return RedirectToAction(nameof(Edit), new { id });
                    }
                    if (!validationResult.HasProperNameLength)
                    {
                        this.toaster.AddWarningToastMessage(ToastrConsts.WrongNameLength);
                        return RedirectToAction(nameof(Edit), new { id });
                    }
                    if (!validationResult.HasValidName)
                    {
                        this.toaster.AddWarningToastMessage(ToastrConsts.NameNotValid);
                        return RedirectToAction(nameof(Edit), new { id });
                    }

                    await this.ingredientService.UpdateIngredientAsync(id, ingredientDTO);

                    this.toaster.AddSuccessToastMessage(ToastrConsts.Success);
                    return RedirectToAction("Details", "Ingredients", new { id });
                }
                catch (ArgumentException)
                {
                    this.toaster.AddWarningToastMessage(ToastrConsts.GenericError);
                    return RedirectToAction(nameof(Index));
                }
            }

            this.toaster.AddWarningToastMessage(ToastrConsts.IncorrectInput);
            return RedirectToAction(nameof(Edit), new { id });
        }

        [HttpGet]
        [Authorize(Roles = "Cocktail Magician")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var ingredientDTO = await this.ingredientService.GetIngredientAsync(id);

                if (ingredientDTO == null)
                {
                    this.toaster.AddWarningToastMessage(ToastrConsts.NotFound);
                    return RedirectToAction(nameof(Index));
                }

                var ingredientVM = this.ingredientDTOMapper.MapToVMFromDTO(ingredientDTO);

                return View(ingredientVM);
            }
            catch (Exception)
            {
                this.toaster.AddWarningToastMessage(ToastrConsts.GenericError);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cocktail Magician")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await this.ingredientService.DeleteIngredientAsync(id);

                this.toaster.AddSuccessToastMessage(ToastrConsts.Success);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException e)
            {
                this.toaster.AddWarningToastMessage(e.Message);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                this.toaster.AddWarningToastMessage(ToastrConsts.GenericError);
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ListAllIngredients()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;

                int totalIngredients = this.ingredientService.GetAllIngredientsCount();
                int filteredIngredients = this.ingredientService.GetAllFilteredIngredientsCount(searchValue);
                var ingredients = await this.ingredientService.ListAllIngredientsAsync(skip, pageSize, searchValue,
                    sortColumn, sortColumnDirection);

                var ingredientsVMs = ingredients.Select(ing => this.ingredientDTOMapper.MapToVMFromDTO(ing)).ToList();

                foreach (var item in ingredientsVMs)
                {
                    item.Cocktails = (await this.cocktailService.GetAllCocktailssAsync())
                        .Where(cocktail => cocktail.Ingredients.Any(x => x.Name == item.Name))
                        .Select(cdto => cocktailDTOMapper.MapToVMFromDTO(cdto)).ToList();
                    item.CocktailNames = string.Join(", ", item.Cocktails.Select(c => c.Name));
                }

                return Json(new { draw = draw, recordsFiltered = filteredIngredients, recordsTotal = totalIngredients, data = ingredientsVMs }); //data = model
            }
            catch (Exception)
            {
                this.toaster.AddWarningToastMessage(ToastrConsts.GenericError);
                return RedirectToAction(nameof(Index));
            }
        }
        //private bool IngredientExists(int id)
        //{
        //    return this.ingredientService.GetAllIngredientsAsync().Result.Any(e => e.Id == id);
        //}
    }
}
