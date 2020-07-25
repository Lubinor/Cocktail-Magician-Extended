using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CocktailMagician.Services.Contracts;
using CocktailMagician.Web.Mappers.Contracts;
using System.Linq.Dynamic;
using CocktailMagician.Web.Models;
using Microsoft.AspNetCore.Authorization;
using NToastNotify;
using CocktailMagician.Web.Utilities;
using Ganss.XSS;

namespace CocktailMagician.Web.Controllers
{
    public class CitiesController : Controller
    {
        private readonly ICityService cityService;
        private readonly IBarService barService;
        private readonly ICityDTOMapper cityMapper;
        private readonly IBarDTOMapper barMapper;
        private readonly IToastNotification toaster;

        public CitiesController(
            ICityService cityService,
            IBarService barService,
            ICityDTOMapper cityMapper,
            IBarDTOMapper barMapper,
            IToastNotification toaster)
        {
            this.cityService = cityService ?? throw new ArgumentNullException(nameof(cityService));
            this.barService = barService ?? throw new ArgumentNullException(nameof(barService));
            this.cityMapper = cityMapper ?? throw new ArgumentNullException(nameof(cityMapper));
            this.barMapper = barMapper ?? throw new ArgumentNullException(nameof(barMapper));
            this.toaster = toaster ?? throw new ArgumentNullException(nameof(toaster));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            try
            {
                var cityDTOs = await this.cityService.GetAllCitiesAsync();

                var cities = cityDTOs
                    .Select(c => cityMapper.MapToVMFromDTO(c))
                    .ToList();

                return View(cities);
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
                var cityDTO = await cityService.GetCityAsync(id);

                if (cityDTO == null)
                {
                    this.toaster.AddWarningToastMessage(ToastrConsts.NotFound);
                    return RedirectToAction(nameof(Index));
                }

                var city = cityMapper.MapToVMFromDTO(cityDTO);

                return View(city);
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
        public async Task<IActionResult> Create([Bind("Name")] CityViewModel cityVM)
        {
            var sanitizer = new HtmlSanitizer();
            cityVM.Name = sanitizer.Sanitize(cityVM.Name);

            if (ModelState.IsValid)
            {
                try
                {
                    var cityDTO = cityMapper.MapToDTOFromVM(cityVM);

                    var validationResult = this.cityService.ValidateCity(cityDTO);

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

                    bool isUnique = this.cityService.CityIsUnique(cityDTO);

                    if (!isUnique)
                    {
                        this.toaster.AddWarningToastMessage(ToastrConsts.NotUnique);
                        return RedirectToAction(nameof(Create));
                    }

                    var result = await this.cityService.CreateCityAsync(cityDTO);

                    this.toaster.AddSuccessToastMessage(ToastrConsts.Success);
                    return RedirectToAction("Details", "Cities", new { id = result.Id });
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
                var cityDTO = await cityService.GetCityAsync(id);

                if (cityDTO == null)
                {
                    this.toaster.AddWarningToastMessage(ToastrConsts.NotFound);
                    return RedirectToAction(nameof(Index));
                }

                var city = cityMapper.MapToVMFromDTO(cityDTO);

                return View(city);
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
        public async Task<IActionResult> Edit(int id, CityViewModel cityVM)
        {
            var sanitizer = new HtmlSanitizer();
            cityVM.Name = sanitizer.Sanitize(cityVM.Name);

            if (id != cityVM.Id)
            {
                this.toaster.AddWarningToastMessage(ToastrConsts.NotFound);
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var cityDTO = this.cityMapper.MapToDTOFromVM(cityVM);

                    var validationResult = this.cityService.ValidateCity(cityDTO);

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

                    await this.cityService.UpdateCityAsync(id, cityDTO);

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
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var cityDTO = await cityService.GetCityAsync(id);

                if (cityDTO == null)
                {
                    this.toaster.AddWarningToastMessage(ToastrConsts.NotFound);
                    return RedirectToAction(nameof(Index));
                }

                var city = cityMapper.MapToVMFromDTO(cityDTO);

                return View(city);
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
                await this.cityService.DeleteCityAsync(id);
                return RedirectToAction("Index", "Cities", new { area = "" });
            }
            catch (Exception)
            {
                this.toaster.AddWarningToastMessage(ToastrConsts.GenericError);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ListAllCities()
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

                int totalCities = this.cityService.GetAllCitiesCount();
                int filteredCities = this.cityService.GetAllFilteredCitiesCount(searchValue);
                var cities = await this.cityService.ListAllCitiesAsync(skip, pageSize, searchValue,
                    sortColumn, sortColumnDirection);

                var cityVMs = cities.Select(city => this.cityMapper.MapToVMFromDTO(city)).ToList();

                foreach (var city in cityVMs)
                {
                    city.Bars = (await this.barService.GetAllBarsAsync())
                        .Where(b => b.CityName.Contains(city.Name))
                        .Select(b => barMapper.MapToVMFromDTO(b)).ToList();

                    //city.BarNames = string.Join(", ", city.Bars.Select(b => b.Name));
                }

                return Json(new { draw = draw, recordsFiltered = filteredCities, recordsTotal = totalCities, data = cityVMs }); //data = model
            }
            catch (Exception)
            {
                this.toaster.AddWarningToastMessage(ToastrConsts.GenericError);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
