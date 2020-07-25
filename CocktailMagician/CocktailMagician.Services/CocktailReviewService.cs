using CocktailMagician.Data;
using CocktailMagician.Services.Contracts;
using CocktailMagician.Services.DTOs;
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
    public class CocktailReviewService : ICocktailReviewService
    {
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly CocktailMagicianContext context;
        private readonly ICocktailReviewMapper cocktailReviewMapper;

        public CocktailReviewService(IDateTimeProvider dateTimeProvider, CocktailMagicianContext context, ICocktailReviewMapper cocktailReviewMapper)
        {
            this.dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.cocktailReviewMapper = cocktailReviewMapper ?? throw new ArgumentNullException(nameof(cocktailReviewMapper));
        }

        public async Task<ICollection<CocktailReviewDTO>> GetAllCocktailReviewsAsync(int cocktailId)
        {
            var CocktailReviewDTOs = await this.context.CocktailsUsersReviews
                .Include(r => r.Cocktail)
                .Include(r => r.User)
                .Where(r => r.CocktailId == cocktailId && !r.IsDeleted)
                .Select(x => cocktailReviewMapper.MapToCocktailReviewDTO(x))
                .ToListAsync();

            return CocktailReviewDTOs;
        }

        public async Task<ICollection<CocktailReviewDTO>> GetAllUserReviewsAsync(int userId)
        {
            var barReviewDTOs = await this.context.CocktailsUsersReviews
                .Include(r => r.Cocktail)
                .Include(r => r.User)
                .Where(r => r.UserId == userId && !r.IsDeleted)
                .Select(x => cocktailReviewMapper.MapToCocktailReviewDTO(x))
                .ToListAsync();

            return barReviewDTOs;
        }

        public async Task<CocktailReviewDTO> GetCocktailReviewAsync(int cocktailId, int userId)
        {
            var cocktailReview = await this.context.CocktailsUsersReviews
                .Include(r => r.Cocktail)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.CocktailId == cocktailId &&
                                          r.UserId == userId &&
                                         !r.IsDeleted);

            if (cocktailReview == null)
            {
                return null;
            }

            var cocktailReviewDTO = this.cocktailReviewMapper.MapToCocktailReviewDTO(cocktailReview);

            return cocktailReviewDTO;
        }

        public async Task<CocktailReviewDTO> CreateCocktailReviewAsync(CocktailReviewDTO cocktailReviewDTO)
        {
            if (cocktailReviewDTO == null)
            {
                return null;
            };

            var cocktailReview = this.cocktailReviewMapper.MapToCocktailReview(cocktailReviewDTO);
            cocktailReview.CreatedOn = this.dateTimeProvider.GetDateTime();

            this.context.CocktailsUsersReviews.Add(cocktailReview);
            await this.context.SaveChangesAsync();

            var cocktail = await this.context.Cocktails.FirstOrDefaultAsync(c => c.Id == cocktailReview.CocktailId);
            cocktail.AverageRating = GetCocktailRating(cocktail.Id);

            this.context.Cocktails.Update(cocktail);
            await this.context.SaveChangesAsync();

            var newCocktailReviewDTO = await this.GetCocktailReviewAsync(cocktailReview.CocktailId, cocktailReview.UserId);

            return newCocktailReviewDTO;
        }

        public async Task<CocktailReviewDTO> UpdateCocktailReviewAsync(int cocktailId, int userId, CocktailReviewDTO cocktailReviewDTO)
        {
            if (cocktailReviewDTO == null)
            {
                return null;
            };

            var cocktailReview = await this.context.CocktailsUsersReviews
                .FirstOrDefaultAsync(r => r.CocktailId == cocktailId &&
                                          r.UserId == userId &&
                                         !r.IsDeleted);

            if (cocktailReview == null)
            {
                return null;
            }

            cocktailReview.Comment = cocktailReviewDTO.Comment;
            cocktailReview.Rating = cocktailReviewDTO.Rating;

            this.context.CocktailsUsersReviews.Update(cocktailReview);
            await this.context.SaveChangesAsync();

            var cocktail = await this.context.Cocktails.FirstOrDefaultAsync(c => c.Id == cocktailReview.CocktailId);
            cocktail.AverageRating = GetCocktailRating(cocktail.Id);

            this.context.Cocktails.Update(cocktail);
            await this.context.SaveChangesAsync();

            var newCocktailReviewDTO = await this.GetCocktailReviewAsync(cocktailId, userId);

            return newCocktailReviewDTO;
        }

        public async Task<bool> DeleteCocktailReviewAsync(int cocktailId, int userId)
        {
            var cocktailReview = await this.context.CocktailsUsersReviews
                .Include(r => r.Cocktail)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.CocktailId == cocktailId &&
                                          r.UserId == userId &&
                                         !r.IsDeleted);

            if (cocktailReview == null)
            {
                return false;
            }

            cocktailReview.IsDeleted = true;

            this.context.CocktailsUsersReviews.Update(cocktailReview);
            await this.context.SaveChangesAsync();

            var cocktail = await this.context.Cocktails.FirstOrDefaultAsync(c => c.Id == cocktailReview.CocktailId);
            cocktail.AverageRating = GetCocktailRating(cocktail.Id);

            this.context.Cocktails.Update(cocktail);
            await this.context.SaveChangesAsync();

            return true;
        }
        public double GetCocktailRating(int cocktailId)
        {
            var allReviews = this.context.CocktailsUsersReviews
                .Where(c => c.CocktailId == cocktailId && !c.IsDeleted);

            double ratingSum = allReviews.Select(r => r.Rating).Sum();

            double averageRating = 0.00;

            if (ratingSum > 0)
            {
                averageRating = (ratingSum * 1.00) / allReviews.Count();
            }

            averageRating = Math.Round(averageRating, 2);

            return averageRating;
        }

        public ValidationModel ValidateCocktailReview(CocktailReviewDTO cocktailReviewDTO)
        {
            var validationModel = new ValidationModel();

            if (cocktailReviewDTO == null)
            {
                validationModel.HasProperInputData = false;
            }
            if (cocktailReviewDTO.Rating < 1 || cocktailReviewDTO.Rating > 5)
            {
                validationModel.HasCorrectRating = false;
            }
            if (cocktailReviewDTO.Comment.Length > 500)
            {
                validationModel.HasCorrectCommentLength = false;
            }
            return validationModel;
        }

        public bool CocktailReviewIsUnique(CocktailReviewDTO cocktailReviewDTO)
        {
            if (this.context.CocktailsUsersReviews
                .Any(x => x.UserId.Equals(cocktailReviewDTO.AuthorId) && 
                          x.CocktailId.Equals(cocktailReviewDTO.CocktailId)))
            {
                return false;
            }

            return true;
        }
    }
}
