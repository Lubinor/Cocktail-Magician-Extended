using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CocktailMagician.Models;
using CocktailMagician.Services.Contracts;
using CocktailMagician.Web.Mappers.Contracts;
using CocktailMagician.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Ganss.XSS;
using NToastNotify;
using CocktailMagician.Web.Utilities;

namespace CocktailMagician.Web.Controllers
{
    public class CocktailReviewsController : Controller
    {
        private readonly ICocktailReviewService cocktailReviewService;
        private readonly ICocktailService cocktailService;
        private readonly IUserService userService;
        private readonly ICocktailDTOMapper cocktailMapper;
        private readonly ICocktailReviewDTOMapper cocktailReviewMapper;
        private readonly UserManager<User> userManager;
        private readonly IToastNotification toaster;

        public CocktailReviewsController(
            ICocktailReviewService cocktailReviewService,
            ICocktailService cocktailService,
            IUserService userService,
            ICocktailDTOMapper cocktailMapper,
            ICocktailReviewDTOMapper cocktailReviewMapper,
            UserManager<User> userManager,
            IToastNotification toaster)
        {
            this.cocktailReviewService = cocktailReviewService ?? throw new ArgumentNullException(nameof(cocktailReviewService));
            this.cocktailService = cocktailService ?? throw new ArgumentNullException(nameof(cocktailService));
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
            this.cocktailMapper = cocktailMapper ?? throw new ArgumentNullException(nameof(cocktailMapper));
            this.cocktailReviewMapper = cocktailReviewMapper ?? throw new ArgumentNullException(nameof(cocktailReviewMapper));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.toaster = toaster ?? throw new ArgumentNullException(nameof(toaster));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> CocktailReviews(int id)
        {
            try
            {
                var cocktailReviewDTOs = await this.cocktailReviewService.GetAllCocktailReviewsAsync(id);
                var cocktailReviewVMs = new ListCocktailReviewsViewModel
                {
                    AllCocktailReviews = cocktailReviewDTOs
                    .Select(c => cocktailReviewMapper.MapToVMFromDTO(c))
                    .ToList()
                };
                cocktailReviewVMs.CocktailId = id;

                return View(cocktailReviewVMs);
            }
            catch (Exception)
            {
                this.toaster.AddWarningToastMessage(ToastrConsts.GenericError);
                return RedirectToAction("Index", "Cocktails", new { id });
            }
        }

        [HttpGet]
        public async Task<IActionResult> UserReviews(int id)
        {
            try
            {
                var userReviewDTOs = await this.cocktailReviewService.GetAllUserReviewsAsync(id);
                var userReviewVMs = new ListCocktailReviewsViewModel
                {
                    AllCocktailReviews = userReviewDTOs
                    .Select(c => cocktailReviewMapper.MapToVMFromDTO(c))
                    .ToList()
                };

                return View(userReviewVMs);
            }
            catch (Exception)
            {
                this.toaster.AddWarningToastMessage(ToastrConsts.GenericError);
                return RedirectToAction("Index", "Cocktails", new { area = "" });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Bar Crawler")]
        public async Task<IActionResult> Create(int id)
        {
            try
            {
                var cocktailDTO = await this.cocktailService.GetCocktailAsync(id);
                //var cocktailVM = cocktailMapper.MapToVMFromDTO(cocktailDTO);

                var currentUserId = int.Parse(userManager.GetUserId(HttpContext.User));

                var reviewVM = new CreateCocktailReviewViewModel
                {
                    CocktailId = cocktailDTO.Id,
                    AuthorId = currentUserId
                };

                //ViewData["BarId"] = new SelectList(_context.Bars, "Id", "Address");
                //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
                return View(reviewVM);
            }
            catch (Exception)
            {
                this.toaster.AddWarningToastMessage(ToastrConsts.GenericError);
                return RedirectToAction("Index", "Cocktails", new { id });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Bar Crawler")]
        public async Task<IActionResult> Create(int id, CreateCocktailReviewViewModel reviewVM)
        {
            var sanitizer = new HtmlSanitizer();
            reviewVM.Comment = sanitizer.Sanitize(reviewVM.Comment);

            if (ModelState.IsValid)
            try
            {
                if (ModelState.IsValid)
                {
                    var currentUserId = int.Parse(userManager.GetUserId(HttpContext.User));
                    reviewVM.AuthorId = currentUserId;
                    //var userDTO = await this.userService.GetUserAsync(currentUserId);
                    //var barDTO = await this.barService.GetBarAsync(reviewVM.BarId);

                    var reviewDTO = this.cocktailReviewMapper.MapToDTOFromVM(reviewVM);
                    reviewDTO.AuthorId = currentUserId;
                    reviewDTO.CocktailId = id;

                    bool isUnique = this.cocktailReviewService.CocktailReviewIsUnique(reviewDTO);

                    if (!isUnique)
                    {
                        this.toaster.AddWarningToastMessage(ToastrConsts.ReviewExists);
                        return RedirectToAction("Index", "Cocktails", new { id = reviewDTO.CocktailId });
                    }

                    var validationResult = this.cocktailReviewService.ValidateCocktailReview(reviewDTO);

                    if (!validationResult.HasProperInputData)
                    {
                        this.toaster.AddWarningToastMessage(ToastrConsts.NullModel);
                        return RedirectToAction(nameof(Create), new { id });
                    }
                    if (!validationResult.HasCorrectRating)
                    {
                        this.toaster.AddWarningToastMessage(ToastrConsts.IncorrectRating);
                        return RedirectToAction(nameof(Create), new { id });
                    }
                    if (!validationResult.HasCorrectCommentLength)
                    {
                        this.toaster.AddWarningToastMessage(ToastrConsts.CommentTooLong);
                        return RedirectToAction(nameof(Create), new { id });
                    }

                    var result = await this.cocktailReviewService.CreateCocktailReviewAsync(reviewDTO);

                    this.toaster.AddSuccessToastMessage(ToastrConsts.Success);
                    return RedirectToAction("Details", "Cocktails", new { id = result.CocktailId });
                }

            }
            catch (Exception)
            {
                this.toaster.AddWarningToastMessage(ToastrConsts.GenericError);
                return RedirectToAction("Index", "Cocktails", new { area = "" });
            }

            this.toaster.AddWarningToastMessage(ToastrConsts.IncorrectInput);
            return RedirectToAction("Index", "Cocktails", new { area = "" });
        }

        [HttpGet]
        [Authorize(Roles = "Bar Crawler")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var currentUserId = int.Parse(userManager.GetUserId(HttpContext.User));

                var reviewDTO = await this.cocktailReviewService.GetCocktailReviewAsync(id, currentUserId);

                if (reviewDTO.AuthorId != currentUserId)
                {
                    this.toaster.AddWarningToastMessage("You are allowed to edit/delete only your own reviews!");
                    return RedirectToAction("Index", "Cocktails", new { area = "" });
                }

                if (reviewDTO == null)
                {
                    this.toaster.AddWarningToastMessage(ToastrConsts.NotFound);
                    return RedirectToAction("Index", "Cocktails", new { area = "" });
                }

                var reviewVM = this.cocktailReviewMapper.MapToVMFromDTO(reviewDTO);
                return View(reviewVM);
            }
            catch (Exception)
            {
                this.toaster.AddWarningToastMessage(ToastrConsts.GenericError);
                return RedirectToAction("Index", "Cocktails", new { area = "" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Bar Crawler")]
        public async Task<IActionResult> Edit(int id, CocktailReviewViewModel reviewVM)
        {
            var sanitizer = new HtmlSanitizer();
            reviewVM.Comment = sanitizer.Sanitize(reviewVM.Comment);

            if (ModelState.IsValid)
            {
                try
                {
                    var currentUserId = int.Parse(userManager.GetUserId(HttpContext.User));

                    var reviewDTO = this.cocktailReviewMapper.MapToDTOFromVM(reviewVM);

                    var validationResult = this.cocktailReviewService.ValidateCocktailReview(reviewDTO);

                    if (!validationResult.HasProperInputData)
                    {
                        this.toaster.AddWarningToastMessage(ToastrConsts.NullModel);
                        return RedirectToAction("Edit", "CocktailReviews", new { id });
                    }
                    if (!validationResult.HasCorrectRating)
                    {
                        this.toaster.AddWarningToastMessage(ToastrConsts.IncorrectRating);
                        return RedirectToAction("Edit", "CocktailReviews", new { id });
                    }
                    if (!validationResult.HasCorrectCommentLength)
                    {
                        this.toaster.AddWarningToastMessage(ToastrConsts.CommentTooLong);
                        return RedirectToAction("Edit", "CocktailReviews", new { id });
                    }

                    var result = await this.cocktailReviewService.UpdateCocktailReviewAsync(id, currentUserId, reviewDTO);

                    this.toaster.AddSuccessToastMessage(ToastrConsts.Success);
                    return RedirectToAction("Details", "Cocktails", new { id });
                }
                catch (Exception)
                {
                    this.toaster.AddWarningToastMessage(ToastrConsts.GenericError);
                    return RedirectToAction("Index", "Cocktails", new { area = "" });
                }
            }
            this.toaster.AddWarningToastMessage(ToastrConsts.IncorrectInput);
            return RedirectToAction("Index", "Cocktails", new { area = "" });
        }

        [HttpGet]
        [Authorize(Roles = "Bar Crawler")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var currentUserId = int.Parse(userManager.GetUserId(HttpContext.User));

                var reviewDTO = await this.cocktailReviewService.GetCocktailReviewAsync(id, currentUserId);

                if (reviewDTO == null)
                {
                    this.toaster.AddWarningToastMessage(ToastrConsts.NotFound);
                    return RedirectToAction("Index", "Cocktails", new { area = "" });
                }

                var reviewVM = this.cocktailReviewMapper.MapToVMFromDTO(reviewDTO);

                return View(reviewVM);
            }
            catch (Exception)
            {
                this.toaster.AddWarningToastMessage(ToastrConsts.GenericError);
                return RedirectToAction("Index", "Cocktails", new { area = "" });
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Bar Crawler")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var currentUserId = int.Parse(userManager.GetUserId(HttpContext.User));

                await this.cocktailReviewService.DeleteCocktailReviewAsync(id, currentUserId);

                this.toaster.AddSuccessToastMessage(ToastrConsts.Success);
                return RedirectToAction("CocktailReviews", "CocktailReviews", new { id });
            }
            catch (Exception)
            {
                this.toaster.AddWarningToastMessage(ToastrConsts.GenericError);
                return RedirectToAction("Index", "Cocktails", new { area = "" });
            }
        }
    }
}
