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
using System.Threading.Tasks;

namespace CocktailMagician.Services
{
    public class BarReviewService : IBarReviewService
    {
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly CocktailMagicianContext context;
        private readonly IBarReviewMapper barReviewMapper;

        public BarReviewService(IDateTimeProvider dateTimeProvider, CocktailMagicianContext context, IBarReviewMapper barReviewMapper)
        {
            this.dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.barReviewMapper = barReviewMapper ?? throw new ArgumentNullException(nameof(barReviewMapper));
        }

        public async Task<ICollection<BarReviewDTO>> GetAllBarReviewsAsync(int barId)
        {
            var barReviewDTOs = await this.context.BarsUsersReviews
                .Include(r => r.Bar)
                .Include(r => r.User)
                .Where(r => r.BarId == barId && !r.IsDeleted)
                .Select(x => barReviewMapper.MapToBarReviewDTO(x))
                .ToListAsync();

            return barReviewDTOs;
        }

        public async Task<ICollection<BarReviewDTO>> GetAllUserReviewsAsync(int userId)
        {
            var barReviewDTOs = await this.context.BarsUsersReviews
                .Include(r => r.Bar)
                .Include(r => r.User)
                .Where(r => r.UserId == userId && !r.IsDeleted)
                .Select(x => barReviewMapper.MapToBarReviewDTO(x))
                .ToListAsync();

            return barReviewDTOs;
        }

        public async Task<BarReviewDTO> GetBarReviewAsync(int barId, int userId)
        {
            var barReview = await this.context.BarsUsersReviews
                .Include(r => r.Bar)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.BarId == barId &&
                                          r.UserId == userId &&
                                         !r.IsDeleted);

            if (barReview == null)
            {
                return null;
            }

            var barReviewDTO = this.barReviewMapper.MapToBarReviewDTO(barReview);

            return barReviewDTO;
        }

        public async Task<BarReviewDTO> CreateBarReviewAsync(BarReviewDTO barReviewDTO)
        {
            if (barReviewDTO == null)
            {
                return null;
            };

            var barReview = this.barReviewMapper.MapToBarReview(barReviewDTO);
            barReview.CreatedOn = this.dateTimeProvider.GetDateTime();

            this.context.BarsUsersReviews.Add(barReview);
            await this.context.SaveChangesAsync();

            var bar = await this.context.Bars.FirstOrDefaultAsync(b => b.Id == barReview.BarId);
            bar.AverageRating = GetBarRating(bar.Id);

            this.context.Bars.Update(bar);
            await this.context.SaveChangesAsync();

            var newBarReviewDTO = await this.GetBarReviewAsync(barReview.BarId, barReview.UserId);

            return newBarReviewDTO;
        }

        public async Task<BarReviewDTO> UpdateBarReviewAsync(int barId, int userId, BarReviewDTO barReviewDTO)
        {
            if (barReviewDTO == null)
            {
                return null;
            };

            var barReview = await this.context.BarsUsersReviews
                .FirstOrDefaultAsync(r => r.BarId == barId &&
                                          r.UserId == userId &&
                                         r.IsDeleted==false);

            if (barReview == null)
            {
                return null;
            }

            barReview.Comment = barReviewDTO.Comment;
            barReview.Rating = barReviewDTO.Rating;

            this.context.BarsUsersReviews.Update(barReview);
            await this.context.SaveChangesAsync();

            var bar = await this.context.Bars.FirstOrDefaultAsync(b => b.Id == barReview.BarId);
            bar.AverageRating = GetBarRating(bar.Id);

            this.context.Bars.Update(bar);
            await this.context.SaveChangesAsync();

            var newBarReviewDTO = await this.GetBarReviewAsync(barId, userId);

            return newBarReviewDTO;
        }

        public async Task<bool> DeleteBarReviewAsync(int barId, int userId)
        {
            var barReview = await this.context.BarsUsersReviews
                .Include(r => r.Bar)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.BarId == barId &&
                                          r.UserId == userId &&
                                         !r.IsDeleted);

            if (barReview == null)
            {
                return false;
            }

            barReview.IsDeleted = true;


            this.context.BarsUsersReviews.Update(barReview);
            await this.context.SaveChangesAsync();

            var bar = await this.context.Bars.FirstOrDefaultAsync(b => b.Id == barReview.BarId);
            bar.AverageRating = GetBarRating(bar.Id);

            this.context.Bars.Update(bar);
            await this.context.SaveChangesAsync();

            return true;
        }

        public double GetBarRating(int barId)
        {
            var allReviews = this.context.BarsUsersReviews
                .Where(b => b.BarId == barId &&
                           !b.IsDeleted);

            double ratingSum = allReviews.Select(r => r.Rating).Sum();

            double averageRating = 0.00;

            if (ratingSum > 0)
            {
                averageRating = (ratingSum * 1.00) / allReviews.Count();
            }

            averageRating = Math.Round(averageRating, 2);

            return averageRating;
        }

        public ValidationModel ValidateBarReview(BarReviewDTO barReviewDTO)
        {
            var validationModel = new ValidationModel();

            if (barReviewDTO == null)
            {
                validationModel.HasProperInputData = false;
            }
            if (barReviewDTO.Rating < 1 || barReviewDTO.Rating > 5)
            {
                validationModel.HasCorrectRating = false;
            }
            if (barReviewDTO.Comment.Length > 500)
            {
                validationModel.HasCorrectCommentLength = false;
            }
            return validationModel;
        }
        public bool BarReviewIsUnique(BarReviewDTO barReviewDTO)
        {
            if (this.context.BarsUsersReviews.Any(x => x.UserId.Equals(barReviewDTO.AuthorId) && x.BarId.Equals(barReviewDTO.BarId)))
            {
                return false;
            }
            return true;
        }
    }
}
