using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CocktailMagician.Services.Contracts;
using CocktailMagician.Web.Mappers.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using CocktailMagician.Web.Models;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using NToastNotify;
using CocktailMagician.Web.Utilities;
using Ganss.XSS;
using Microsoft.AspNetCore.Identity;
using CocktailMagician.Models;

namespace CocktailMagician.Web.Controllers
{
    public class CocktailsController : Controller
    {
        private readonly ICocktailService cocktailService;
        private readonly IIngredientService ingredientService;
        private readonly IBarService barService;
        private readonly ICocktailDTOMapper cocktailDTOMapper;
        private readonly IIngredientDTOMapper ingredientDTOMapper;
        private readonly IBarDTOMapper barDTOMApper;
        private readonly IToastNotification toaster;
        private readonly UserManager<User> userManager;

        public CocktailsController(
            ICocktailService cocktailService,
            IIngredientService ingredientService,
            IBarService barService,
            ICocktailDTOMapper cocktailDTOMapper,
            IIngredientDTOMapper ingredientDTOMapper,
            IBarDTOMapper barDTOMApper,
            IToastNotification toaster,
            UserManager<User> userManager
            )
        {
            this.cocktailService = cocktailService ?? throw new ArgumentNullException(nameof(cocktailService));
            this.ingredientService = ingredientService ?? throw new ArgumentNullException(nameof(ingredientService));
            this.barService = barService ?? throw new ArgumentNullException(nameof(barService));
            this.cocktailDTOMapper = cocktailDTOMapper ?? throw new ArgumentNullException(nameof(cocktailDTOMapper));
            this.ingredientDTOMapper = ingredientDTOMapper ?? throw new ArgumentNullException(nameof(ingredientDTOMapper));
            this.barDTOMApper = barDTOMApper ?? throw new ArgumentNullException(nameof(barDTOMApper));
            this.toaster = toaster ?? throw new ArgumentNullException(nameof(toaster));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            try
            {
                var cocktailDTOs = await this.cocktailService.GetAllCocktailssAsync();
                var cocktailVMs = cocktailDTOs.Select(cocktailDTO =>
                    cocktailDTOMapper.MapToVMFromDTO(cocktailDTO)).ToList();

                foreach (var cocktail in cocktailVMs)
                {
                    cocktail.Ingredients = (await this.ingredientService.GetAllIngredientsAsync())
                        .Where(ingredient => ingredient.CocktailDTOs.Any(c => c.Name == cocktail.Name))
                        .Select(idto => ingredientDTOMapper.MapToVMFromDTO(idto)).ToList();
                    cocktail.Bars = (await this.barService.GetAllBarsAsync())
                        .Where(bar => bar.Cocktails.Any(c => c.Name == cocktail.Name))
                        .Select(bdto => barDTOMApper.MapToVMFromDTO(bdto)).ToList();
                }

                return View(cocktailVMs);
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
                var cocktailDTO = await this.cocktailService.GetCocktailAsync(id);

                if (cocktailDTO == null)
                {
                    this.toaster.AddWarningToastMessage(ToastrConsts.NotFound);
                    return RedirectToAction(nameof(Index));
                }

                var cocktailVM = this.cocktailDTOMapper.MapToVMFromDTO(cocktailDTO);

                return View(cocktailVM);
            }
            catch (Exception)
            {
                this.toaster.AddWarningToastMessage(ToastrConsts.GenericError);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        [Authorize(Roles = "Cocktail Magician")]
        public async Task<IActionResult> Create()
        {
            try
            {
                var ingredients = await this.ingredientService.GetAllIngredientsAsync();
                var ingredientVMs = ingredients.Select(ingredient => ingredientDTOMapper.MapToVMFromDTO(ingredient));

                ViewData["Ingredients"] = new MultiSelectList(ingredientVMs, "Id", "Name");
                return View();
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
        public async Task<IActionResult> Create(CreateCocktailViewModel createCocktailViewModel)
        {
            var sanitizer = new HtmlSanitizer();
            createCocktailViewModel.Name = sanitizer.Sanitize(createCocktailViewModel.Name);

            if (createCocktailViewModel.File == null)
            {
                this.toaster.AddWarningToastMessage(ToastrConsts.NoPicture);
                return RedirectToAction(nameof(Create));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (createCocktailViewModel.File.Length > 0)
                    {
                        MemoryStream ms = new MemoryStream();
                        await createCocktailViewModel.File.CopyToAsync(ms);
                        createCocktailViewModel.ImageData = ms.ToArray();

                        ms.Close();
                        ms.Dispose();
                    }

                    var cocktailDTO = this.cocktailDTOMapper.MapToDTOFromVM(createCocktailViewModel);

                    var validationResult = this.cocktailService.ValidateCocktail(cocktailDTO);

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

                    bool isUnique = this.cocktailService.CocktailIsUnique(cocktailDTO);

                    if (!isUnique)
                    {
                        this.toaster.AddWarningToastMessage(ToastrConsts.NotUnique);
                        return RedirectToAction(nameof(Create));
                    }

                    var currentUserId = int.Parse(userManager.GetUserId(HttpContext.User));
                    cocktailDTO.CreatorId = currentUserId;

                    await this.cocktailService.CreateCocktailAsync(cocktailDTO);

                    this.toaster.AddSuccessToastMessage(ToastrConsts.Success);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    this.toaster.AddWarningToastMessage(ToastrConsts.GenericError);
                    return RedirectToAction(nameof(Create));
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
                var ingredients = await this.ingredientService.GetAllIngredientsAsync();
                var ingredientVMs = ingredients.Select(ingredient => ingredientDTOMapper.MapToVMFromDTO(ingredient));
                var bars = await this.barService.GetAllBarsAsync();
                var barVMs = bars.Select(bar => barDTOMApper.MapToVMFromDTO(bar));

                var cocktailDTO = await this.cocktailService.GetCocktailAsync(id); ;

                if (cocktailDTO == null)
                {
                    this.toaster.AddWarningToastMessage(ToastrConsts.NotFound);
                    return RedirectToAction(nameof(Index));
                }

                var editCocktailVM = new EditCocktailViewModel
                {
                    Id = cocktailDTO.Id,
                    Name = cocktailDTO.Name,
                    ContainedBars = cocktailDTO.Bars.Select(b => b.Id).ToList(),
                    ContainedIngredients = cocktailDTO.Ingredients.Select(i => i.Id).ToList(),
                };

                ViewData["Ingredients"] = new MultiSelectList(ingredientVMs, "Id", "Name");
                ViewData["Bars"] = new MultiSelectList(barVMs, "Id", "Name");

                return View(editCocktailVM);
            }
            catch (Exception)
            {
                this.toaster.AddWarningToastMessage(ToastrConsts.GenericError);
                return RedirectToAction(nameof(Edit), new { id });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cocktail Magician")]
        public async Task<IActionResult> Edit(int id, EditCocktailViewModel newEditCocktailVM)
        {
            var sanitizer = new HtmlSanitizer();
            newEditCocktailVM.Name = sanitizer.Sanitize(newEditCocktailVM.Name);

            if (id != newEditCocktailVM.Id)
            {
                this.toaster.AddWarningToastMessage(ToastrConsts.NotFound);
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (newEditCocktailVM.File != null)
                    {
                        MemoryStream ms = new MemoryStream();
                        await newEditCocktailVM.File.CopyToAsync(ms);
                        newEditCocktailVM.ImageData = ms.ToArray();

                        ms.Close();
                        ms.Dispose();
                    }

                    var cocktailDTO = cocktailDTOMapper.MapToDTOFromVM(newEditCocktailVM);

                    var validationResult = this.cocktailService.ValidateCocktail(cocktailDTO);

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

                    await this.cocktailService.UpdateCocktailAsync(id, cocktailDTO);

                    this.toaster.AddSuccessToastMessage(ToastrConsts.Success);
                    return RedirectToAction(nameof(Details), new { id });
                }
                catch (Exception)
                {
                    this.toaster.AddWarningToastMessage(ToastrConsts.GenericError);
                    return RedirectToAction(nameof(Edit), new { id });
                }
            }
            this.toaster.AddWarningToastMessage(ToastrConsts.IncorrectInput);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Cocktail Magician")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var cocktailDTO = await this.cocktailService.GetCocktailAsync(id);

                if (cocktailDTO == null)
                {
                    this.toaster.AddWarningToastMessage(ToastrConsts.NotFound);
                    return RedirectToAction(nameof(Index));
                }

                var cocktailVM = this.cocktailDTOMapper.MapToVMFromDTO(cocktailDTO);

                return View(cocktailVM);
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
                await this.cocktailService.DeleteCocktailAsync(id);

                this.toaster.AddSuccessToastMessage(ToastrConsts.Success);
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
        public async Task<IActionResult> ListAllCocktails()
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

                int totalCocktails = this.cocktailService.GetAllCocktailsCount();
                int filteredCocktails = this.cocktailService.GetAllFilteredCocktailsCount(searchValue);
                var cocktails = await this.cocktailService.ListAllCocktailsAsync(skip, pageSize, searchValue,
                    sortColumn, sortColumnDirection);

                var cocktailVMs = cocktails.Select(cocktail => this.cocktailDTOMapper.MapToVMFromDTO(cocktail)).ToList();

                foreach (var item in cocktailVMs)
                {
                    item.Ingredients = (await this.ingredientService.GetAllIngredientsAsync())
                        .Where(ingredient => ingredient.CocktailDTOs.Any(x => x.Name == item.Name))
                        .Select(idto => ingredientDTOMapper.MapToVMFromDTO(idto)).ToList();
                    item.Bars = (await this.barService.GetAllBarsAsync())
                        .Where(bar => bar.Cocktails.Any(x => x.Name == item.Name))
                        .Select(bdto => barDTOMApper.MapToVMFromDTO(bdto)).ToList();
                    item.IngredientNames = string.Join(", ", item.Ingredients.Select(i => i.Name));
                    item.BarNames = string.Join(", ", item.Bars.Select(b => b.Name));
                    //item.CocktailNames = string.Join(", ", item.Cocktails.Select(c => c.Name)); ingredientNames i BarNames
                }

                //return Json(new { draw = draw, recordsFiltered = filteredCocktails, recordsTotal = totalCocktails, data = cocktailVMs }); //data = model

                var json = Json(new { draw = draw, recordsFiltered = filteredCocktails, recordsTotal = totalCocktails, data = cocktailVMs });
                return json;
            }
            catch (Exception e)
            {
                this.toaster.AddWarningToastMessage(ToastrConsts.GenericError);
                return RedirectToAction(nameof(Index));
            }
        }
        //private bool CocktailExists(int id)
        //{
        //    return this.cocktailService.GetAllCocktailssAsync().Result.Any(e => e.Id == id);
        //}
    }
}
