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
    public class BarReviewsController : Controller
    {
        private readonly IBarReviewService barReviewService;
        private readonly IBarService barService;
        private readonly IUserService userService;
        private readonly IBarDTOMapper barMapper;
        private readonly IBarReviewDTOMapper barReviewMapper;
        private readonly UserManager<User> userManager;
        private readonly IToastNotification toaster;

        public BarReviewsController(
            IBarReviewService barReviewService,
            IBarService barService,
            IUserService userService,
            IBarDTOMapper barMapper,
            IBarReviewDTOMapper barReviewMapper,
            UserManager<User> userManager,
            IToastNotification toaster)
        {
            this.barReviewService = barReviewService ?? throw new ArgumentNullException(nameof(barReviewService));
            this.barService = barService ?? throw new ArgumentNullException(nameof(barService));
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
            this.barMapper = barMapper ?? throw new ArgumentNullException(nameof(barMapper));
            this.barReviewMapper = barReviewMapper ?? throw new ArgumentNullException(nameof(barReviewMapper));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.toaster = toaster ?? throw new ArgumentNullException(nameof(toaster));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> BarReviews(int id)
        {
            try
            {
                var barReviewDTOs = await this.barReviewService.GetAllBarReviewsAsync(id);
                var barReviewVMs = new ListBarReviewsViewModel
                {
                    AllBarReviews = barReviewDTOs
                    .Select(br => barReviewMapper.MapToVMFromDTO(br))
                    .ToList()
                };
                barReviewVMs.BarId = id;

                return View(barReviewVMs);
            }
            catch (Exception)
            {
                this.toaster.AddWarningToastMessage(ToastrConsts.GenericError);
                return RedirectToAction("Index", "Bars", new { id });
            }
        }

        [HttpGet]
        public async Task<IActionResult> UserReviews(int id)
        {
            try
            {
                var userReviewDTOs = await this.barReviewService.GetAllUserReviewsAsync(id);
                var userReviewVMs = new ListBarReviewsViewModel
                {
                    AllBarReviews = userReviewDTOs
                    .Select(br => barReviewMapper.MapToVMFromDTO(br))
                    .ToList()
                };

                return View(userReviewVMs);
            }
            catch (Exception)
            {
                this.toaster.AddWarningToastMessage(ToastrConsts.GenericError);
                return RedirectToAction("Index", "Bars", new { area = "" });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Bar Crawler")]
        public async Task<IActionResult> Create(int id)
        {
            try
            {
                var barDTO = await this.barService.GetBarAsync(id);
                var barVM = barMapper.MapToVMFromDTO(barDTO);

                var currentUserId = int.Parse(userManager.GetUserId(HttpContext.User));

                var reviewVM = new BarReviewViewModel
                {
                    BarName = barVM.Name,
                    BarId = barDTO.Id,
                    AuthorId = currentUserId
                };

                //ViewData["BarId"] = new SelectList(_context.Bars, "Id", "Address");
                //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
                return View(reviewVM);
            }
            catch (Exception)
            {
                this.toaster.AddWarningToastMessage(ToastrConsts.GenericError);
                return RedirectToAction("Index", "Bars", new { id });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Bar Crawler")]
        public async Task<IActionResult> Create(int id, BarReviewViewModel reviewVM)
        {
            var sanitizer = new HtmlSanitizer();
            reviewVM.Comment = sanitizer.Sanitize(reviewVM.Comment);

            try
            {
                if (ModelState.IsValid)
                {
                    var currentUserId = int.Parse(userManager.GetUserId(HttpContext.User));

                    //var userDTO = await this.userService.GetUserAsync(currentUserId);
                    //var barDTO = await this.barService.GetBarAsync(reviewVM.BarId);

                    var reviewDTO = this.barReviewMapper.MapToDTOFromVM(reviewVM);
                    reviewDTO.AuthorId = currentUserId;
                    reviewDTO.BarId = id;

                    bool isUnique = this.barReviewService.BarReviewIsUnique(reviewDTO);

                    if (!isUnique)
                    {
                        this.toaster.AddWarningToastMessage(ToastrConsts.ReviewExists);
                        return RedirectToAction("Index", "Bars", new { id = reviewDTO.BarId });
                    }

                    var validationResult = this.barReviewService.ValidateBarReview(reviewDTO);

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

                    var result = await this.barReviewService.CreateBarReviewAsync(reviewDTO);

                    this.toaster.AddSuccessToastMessage(ToastrConsts.Success);
                    return RedirectToAction("BarReviews", "BarReviews", new { id = result.BarId });
                }
            }
            catch (Exception)
            {
                this.toaster.AddWarningToastMessage(ToastrConsts.GenericError);
                return RedirectToAction("Index", "Bars", new { area = "" });
            }

            this.toaster.AddWarningToastMessage(ToastrConsts.IncorrectInput);
            return RedirectToAction("Index", "Bars", new { area = "" });
        }

        [HttpGet]
        [Authorize(Roles = "Bar Crawler")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var currentUserId = int.Parse(userManager.GetUserId(HttpContext.User));

                var reviewDTO = await this.barReviewService.GetBarReviewAsync(id, currentUserId);

                if (reviewDTO.AuthorId != currentUserId)
                {
                    this.toaster.AddWarningToastMessage("You are allowed to edit/delete only your own reviews!");
                    return RedirectToAction("Index", "Bars", new { area = "" });
                }

                if (reviewDTO == null)
                {
                    this.toaster.AddWarningToastMessage(ToastrConsts.NotFound);
                    return RedirectToAction("Index", "Bars", new { area = "" });
                }

                var reviewVM = this.barReviewMapper.MapToVMFromDTO(reviewDTO);

                return View(reviewVM);
            }
            catch (Exception)
            {
                this.toaster.AddWarningToastMessage(ToastrConsts.GenericError);
                return RedirectToAction("Index", "Bars", new { area = "" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Bar Crawler")]
        public async Task<IActionResult> Edit(int id, BarReviewViewModel reviewVM)
        {
            var sanitizer = new HtmlSanitizer();
            reviewVM.Comment = sanitizer.Sanitize(reviewVM.Comment);

            if (ModelState.IsValid)
            {
                try
                {
                    var currentUserId = int.Parse(userManager.GetUserId(HttpContext.User));

                    var reviewDTO = this.barReviewMapper.MapToDTOFromVM(reviewVM);

                    var validationResult = this.barReviewService.ValidateBarReview(reviewDTO);

                    if (!validationResult.HasProperInputData)
                    {
                        this.toaster.AddWarningToastMessage(ToastrConsts.NullModel);
                        return RedirectToAction("Edit", "BarReviews", new { id });
                    }
                    if (!validationResult.HasCorrectRating)
                    {
                        this.toaster.AddWarningToastMessage(ToastrConsts.IncorrectRating);
                        return RedirectToAction("Edit", "BarReviews", new { id });
                    }
                    if (!validationResult.HasCorrectCommentLength)
                    {
                        this.toaster.AddWarningToastMessage(ToastrConsts.CommentTooLong);
                        return RedirectToAction("Edit", "BarReviews", new { id });
                    }

                    var result = await this.barReviewService.UpdateBarReviewAsync(id, currentUserId, reviewDTO);

                    this.toaster.AddSuccessToastMessage(ToastrConsts.Success);
                    return RedirectToAction("BarReviews", "BarReviews", new { id });
                }
                catch (Exception)
                {
                    this.toaster.AddWarningToastMessage(ToastrConsts.GenericError);
                    return RedirectToAction("Index", "Bars", new { area = "" });
                }
            }
            this.toaster.AddWarningToastMessage(ToastrConsts.IncorrectInput);
            return RedirectToAction("Index", "Bars", new { area = "" });
        }

        [HttpGet]
        [Authorize(Roles = "Bar Crawler")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var currentUserId = int.Parse(userManager.GetUserId(HttpContext.User));

                var reviewDTO = await this.barReviewService.GetBarReviewAsync(id, currentUserId);

                if (reviewDTO == null)
                {
                    this.toaster.AddWarningToastMessage(ToastrConsts.NotFound);
                    return RedirectToAction("Index", "Bars", new { area = "" });
                }

                var reviewVM = this.barReviewMapper.MapToVMFromDTO(reviewDTO);

                return View(reviewVM);
            }
            catch (Exception)
            {
                this.toaster.AddWarningToastMessage(ToastrConsts.GenericError);
                return RedirectToAction("Index", "Bars", new { area = "" });
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

                await this.barReviewService.DeleteBarReviewAsync(id, currentUserId);

                this.toaster.AddSuccessToastMessage(ToastrConsts.Success);
                return RedirectToAction("BarReviews", "BarReviews", new { id });
            }
            catch (Exception)
            {
                this.toaster.AddWarningToastMessage(ToastrConsts.GenericError);
                return RedirectToAction("Index", "Bars", new { area = "" });
            }
        }
    }
}
