using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameReviewApi.Data;
using GameReviewApi.Entities;
using GameReviewApi.Helpers;
using GameReviewApi.Services;
using GameReviewApi.Models;

namespace GameReviewApi.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private ReviewContext _context;
        private IPropertyMappingService _propertyMappingService;

        public ReviewRepository(ReviewContext context, IPropertyMappingService propertyMappingService)
        {
            _context = context;
            _propertyMappingService = propertyMappingService;
        }

        public async Task AddReview(Review review)
        {
            await _context.Reviews.AddAsync(review);
        }

        public async Task AddGame(int reviewId, Game game)
        {
            await _context.Games.AddAsync(game);
        }

        public async Task AddComment(int reviewId, Comment comment)
        {
            var review = await GetReviewById(reviewId);

            if (review != null)
            {
                review.Comments.Add(comment);
            }
            await _context.Comments.AddAsync(comment);
        }

        public async Task<Game> GetGamesForReview(int reviewId)
        {
            return await _context.Games.Where(g => g.ReviewId == reviewId).SingleOrDefaultAsync();

            //return await _context.Games
            //    .Where(g => g.ReviewId == reviewId)
            //    .OrderBy(g => g.Title)
            //    .ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetCommentsForReview(int reviewId)
        {
            return await _context.Comments
                .Where(c => c.ReviewId == reviewId)
                .OrderBy(c => c.Author)
                .ToListAsync();
        }

        public async Task<Comment> GetCommentForReview(int reviewId, int commentID)
        {
            return await _context.Comments
                .Where(c => c.ReviewId == reviewId && c.Id == commentID)
                .FirstOrDefaultAsync();
        }

        public async Task<Game> GetGameForReview(int reviewId, int gameId)
        {
            return await _context.Games.Where(g => g.ReviewId == reviewId && g.Id == gameId).FirstOrDefaultAsync();
        }

        public async Task<Review> GetReviewById(int Id)
        {
            return await _context.Reviews.FirstOrDefaultAsync(r => r.Id == Id);
        }

        public async Task<PagedList<Review>> GetAllReviews(ReviewResourceParameters reviewResourceParameters)
        {
            //var collectionBeforePaging = _context.Reviews
            //    .OrderByDescending(r => r.DatePosted).AsQueryable();

            var collectionBeforePaging =
                _context.Reviews.ApplySort(reviewResourceParameters.OrderBy, 
                _propertyMappingService.GetPropertyMapping<ReviewDto,Review>());

            if (!string.IsNullOrEmpty(reviewResourceParameters.ReviewTitle))
            {
                var reviewTitleForWhereClause = reviewResourceParameters.ReviewTitle.Trim().ToLowerInvariant();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.ReviewTitle.ToLowerInvariant() == reviewTitleForWhereClause);
            }

            if (!string.IsNullOrEmpty(reviewResourceParameters.GameTitle))
            {
                var gameTitleForWhereClause = reviewResourceParameters.GameTitle.Trim().ToLowerInvariant();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Game.Title.ToLowerInvariant() == gameTitleForWhereClause);
            }

            if (!string.IsNullOrEmpty(reviewResourceParameters.SearchQuery))
            {
                var searchQueryFromWhereClause = reviewResourceParameters.SearchQuery.Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.ReviewTitle.ToLowerInvariant().Contains(searchQueryFromWhereClause)
                    || a.Author.ToLowerInvariant().Contains(searchQueryFromWhereClause)
                    || a.Game.Title.ToLowerInvariant().Contains(searchQueryFromWhereClause)
                    || a.Game.Publisher.ToLowerInvariant().Contains(searchQueryFromWhereClause)
                    || a.Game.Developer.ToLowerInvariant().Contains(searchQueryFromWhereClause)
                    || a.Game.Genre.ToLowerInvariant().Contains(searchQueryFromWhereClause));
            }

            return await PagedList<Review>.Create(collectionBeforePaging, 
                reviewResourceParameters.PageNumber, 
                reviewResourceParameters.PageSize);

            //return await _context.Reviews
            //    .OrderByDescending(r => r.DatePosted)
            //    .Skip(reviewResourceParameters.PageSize * (reviewResourceParameters.PageNumber - 1))
            //    .Take(reviewResourceParameters.PageSize)
            //    .ToListAsync();
        }

        public async Task DeleteComment(Comment comment)
        {
            var itemToRemove = await _context.Comments.SingleOrDefaultAsync(c => c.Id == comment.Id);
            if (itemToRemove != null)
            {
                _context.Comments.Remove(itemToRemove);
            }
        }

        public async Task DeleteGame(Game game)
        {
            var itemToRemove = await _context.Games.SingleOrDefaultAsync(c => c.Id == game.Id);
            if (itemToRemove != null)
            {
                _context.Games.Remove(itemToRemove);
            }
        }

        public async Task DeleteReview(Review review)
        {
            var itemToRemove = await _context.Reviews.SingleOrDefaultAsync(c => c.Id == review.Id);
            if (itemToRemove != null)
            {
                _context.Reviews.Remove(itemToRemove);
            }
        }

        public async Task UpdateReview(Review review)
        {
            //var itemToUpdate = await _context.Reviews.SingleOrDefaultAsync(r => r.Id == review.Id);

            //if (itemToUpdate != null)
            //{
            //    itemToUpdate.ReviewTitle = review.ReviewTitle;
            //    itemToUpdate.Author = review.Author;
            //    itemToUpdate.VideoUrl = review.VideoUrl;
            //    itemToUpdate.Introduction = review.Introduction;
            //    itemToUpdate.Body = review.Body;
            //    itemToUpdate.Conclusion = review.Conclusion;
            //    itemToUpdate.DatePosted = review.DatePosted;

            //    //itemToUpdate.Game = review.Game;
            //    //itemToUpdate.Comments = review.Comments;
            //    //May or may not be needed
            //}
        }

        public async Task UpdateGame(Game game)
        {
        }

        public async Task UpdateComment(Comment comment)
        {
        }

        public async Task<bool> ReviewExists(int reviewId)
        {
            return await _context.Reviews.AnyAsync(r => r.Id == reviewId);
        }

        public async Task<bool> Save()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
