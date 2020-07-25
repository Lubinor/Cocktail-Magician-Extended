using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CocktailMagician.Services.Contracts;
using CocktailMagician.Web.Mappers.Contracts;
using CocktailMagician.Web.Models;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using NToastNotify;
using CocktailMagician.Web.Utilities;
using Ganss.XSS;

namespace CocktailMagician.Web.Controllers
{
    public class BarsController : Controller
    {
        private readonly IBarService barService;
        private readonly ICityService cityService;
        private readonly ICocktailService cocktailService;
        private readonly IBarDTOMapper barMapper;
        private readonly ICocktailDTOMapper cocktailMapper;
        private readonly ICityDTOMapper citymapper;
        private readonly IToastNotification toaster;

        public BarsController(
            IBarService barService,
            ICityService cityService,
            ICocktailService cocktailService,
            IBarDTOMapper barMapper,
            ICocktailDTOMapper cocktailMapper,
            ICityDTOMapper citymapper,
            IToastNotification toaster)
        {
            this.barService = barService ?? throw new ArgumentNullException(nameof(barService));
            this.cityService = cityService ?? throw new ArgumentNullException(nameof(cityService));
            this.cocktailService = cocktailService ?? throw new ArgumentNullException(nameof(cocktailService));
            this.barMapper = barMapper ?? throw new ArgumentNullException(nameof(barMapper));
            this.cocktailMapper = cocktailMapper ?? throw new ArgumentNullException(nameof(cocktailMapper));
            this.citymapper = citymapper ?? throw new ArgumentNullException(nameof(citymapper));
            this.toaster = toaster ?? throw new ArgumentNullException(nameof(toaster));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            try
            {
                var barDTOs = await this.barService.GetAllBarsAsync();

                var bars = barDTOs
                    .Select(b => barMapper.MapToVMFromDTO(b))
                    .ToList();

                return View(bars);
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
                var barDTO = await barService.GetBarAsync(id);

                if (barDTO == null)
                {
                    this.toaster.AddWarningToastMessage(ToastrConsts.NotFound);
                    return RedirectToAction(nameof(Index));
                }

                var bar = barMapper.MapToVMFromDTO(barDTO);

                return View(bar);
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
                var citiesDTO = await this.cityService.GetAllCitiesAsync();
                var citiesVM = citiesDTO.Select(c => citymapper.MapToVMFromDTO(c)).ToList();

                ViewData["CityId"] = new SelectList(citiesVM, "Id", "Name");

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
        public async Task<IActionResult> Create([Bind("Name", "CityName", "CityId", "Address", "City", "Phone", "File")] BarViewModel barVM)
        {
            var sanitizer = new HtmlSanitizer();
            barVM.Name = sanitizer.Sanitize(barVM.Name);

            if (barVM.File == null)
            {
                this.toaster.AddWarningToastMessage(ToastrConsts.NoPicture);
                return RedirectToAction(nameof(Create));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (barVM.File.Length > 0)
                    {
                        MemoryStream ms = new MemoryStream();
                        await barVM.File.CopyToAsync(ms);
                        barVM.ImageData = ms.ToArray();

                        ms.Close();
                        ms.Dispose();
                    }

                    var barDTO = barMapper.MapToDTOFromVM(barVM);

                    var validationResult = this.barService.ValidateBar(barDTO);

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
                    if (!validationResult.HasProperAddress)
                    {
                        this.toaster.AddWarningToastMessage(ToastrConsts.IncorrectAddress);
                        return RedirectToAction(nameof(Create));
                    }
                    if (!validationResult.HasProperPhone)
                    {
                        this.toaster.AddWarningToastMessage(ToastrConsts.IncorrectPhone);
                        return RedirectToAction(nameof(Create));
                    }

                    bool isUnique = this.barService.BarIsUnique(barDTO);

                    if (!isUnique)
                    {
                        this.toaster.AddWarningToastMessage(ToastrConsts.NotUnique);
                        return RedirectToAction(nameof(Create));
                    }

                    var result = await this.barService.CreateBarAsync(barDTO);

                    this.toaster.AddSuccessToastMessage(ToastrConsts.Success);
                    return RedirectToAction("Details", "Bars", new { id = result.Id });
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
                var barDTO = await barService.GetBarAsync(id);

                if (barDTO == null)
                {
                    this.toaster.AddWarningToastMessage(ToastrConsts.NotFound);
                    return RedirectToAction(nameof(Index));
                }

                var citiesDTO = await this.cityService.GetAllCitiesAsync();
                var citiesVM = citiesDTO.Select(c => citymapper.MapToVMFromDTO(c)).ToList();

                ViewData["CityId"] = new SelectList(citiesVM, "Id", "Name");

                var bar = barMapper.MapToVMFromDTO(barDTO);

                return View(bar);

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
        public async Task<IActionResult> Edit(int id, BarViewModel barVM)
        {
            var sanitizer = new HtmlSanitizer();
            barVM.Name = sanitizer.Sanitize(barVM.Name);

            if (id != barVM.Id)
            {
                this.toaster.AddWarningToastMessage(ToastrConsts.NotFound);
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (barVM.File != null)
                    {
                        MemoryStream ms = new MemoryStream();
                        await barVM.File.CopyToAsync(ms);
                        barVM.ImageData = ms.ToArray();

                        ms.Close();
                        ms.Dispose();
                    }

                    var barDTO = this.barMapper.MapToDTOFromVM(barVM);

                    var validationResult = this.barService.ValidateBar(barDTO);

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
                    if (!validationResult.HasProperAddress)
                    {
                        this.toaster.AddWarningToastMessage(ToastrConsts.IncorrectAddress);
                        return RedirectToAction(nameof(Edit), new { id });
                    }
                    if (!validationResult.HasProperPhone)
                    {
                        this.toaster.AddWarningToastMessage(ToastrConsts.IncorrectPhone);
                        return RedirectToAction(nameof(Edit), new { id });
                    }

                    await this.barService.UpdateBarAsync(id, barDTO);

                    this.toaster.AddSuccessToastMessage(ToastrConsts.Success);
                    return RedirectToAction("Details", "Bars", new { id });
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
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var barDTO = await barService.GetBarAsync(id);

                if (barDTO == null)
                {
                    this.toaster.AddWarningToastMessage(ToastrConsts.NotFound);
                    return RedirectToAction(nameof(Index));
                }

                var bar = barMapper.MapToVMFromDTO(barDTO);

                return View(bar);
            }
            catch (Exception)
            {
                this.toaster.AddWarningToastMessage(ToastrConsts.GenericError);
                return RedirectToAction(nameof(Create));
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cocktail Magician")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await this.barService.DeleteBarAsync(id);

                this.toaster.AddSuccessToastMessage(ToastrConsts.Success);
                return RedirectToAction("Index", "Bars", new { area = "" });
            }
            catch (Exception)
            {
                this.toaster.AddWarningToastMessage(ToastrConsts.GenericError);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ListAllBars()
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

                int totalBars = this.barService.GetAllBarsCount();
                int filteredBars = this.barService.GetAllFilteredBarsCount(searchValue);
                var bars = await this.barService.ListAllBarsAsync(skip, pageSize, searchValue,
                    sortColumn, sortColumnDirection);

                var barVMs = bars.Select(bar => this.barMapper.MapToVMFromDTO(bar)).ToList();

                foreach (var bar in barVMs)
                {
                    bar.Cocktails = (await this.cocktailService.GetAllCocktailssAsync())
                        .Where(c => c.Bars.Any(x => x.Name == bar.Name))
                        .Select(cdto => cocktailMapper.MapToVMFromDTO(cdto)).ToList();
                    //bar.CocktailNames = string.Join(", ", bar.Cocktails.Select(c => c.Name));
                }

                return Json(new { draw = draw, recordsFiltered = filteredBars, recordsTotal = totalBars, data = barVMs }); //data = model
            }
            catch (Exception)
            {
                this.toaster.AddWarningToastMessage(ToastrConsts.GenericError);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
