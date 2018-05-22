using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameReviewApi.Data;
using GameReviewApi.Entities;

namespace GameReviewApi.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        ReviewContext _context;

        public ReviewRepository(ReviewContext context)
        {
            _context = context;
        }

        public async Task AddReview(Review review)
        {
            await _context.Reviews.AddAsync(review);
            //await _context.SaveChangesAsync();
        }

        public async Task AddGame(Game game)
        {
            await _context.Games.AddAsync(game);
            //await _context.SaveChangesAsync();
        }

        public async Task AddComment(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            //await _context.SaveChangesAsync();
        }

        public async Task<Comment> GetCommentById(int Id)
        {
            return await _context.Comments
                .Where(c => c.CommentId == Id)
                .SingleOrDefaultAsync();
        }

        public async Task<Comment> GetCommentsByReviewId(int reviewId, int commentID)
        {
            return await _context.Comments
                .Where(c => c.ReviewId == reviewId && c.CommentId == commentID)
                .FirstOrDefaultAsync();
        }

        public async Task<Game> GetGameById(int Id)
        {
            return await _context.Games
                .Where(g => g.Id == Id)
                .SingleOrDefaultAsync();
        }

        public async Task<Game> GetGameByReviewId(int gameId, int reviewId)
        {
            return await _context.Games.Where(g => g.Id == gameId && g.ReviewId == reviewId).FirstOrDefaultAsync();
        }

        public async Task<Review> GetReviewByGameId(int reviewId, int gameId)
        {
            return await _context.Reviews.Where(r => r.Game.Id == gameId && r.Id == reviewId).FirstOrDefaultAsync();
        }

        public async Task<Review> GetReviewById(int Id)
        {
            return await _context.Reviews
                .Where(r => r.Id == Id)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Comment>> GetAllComments()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetAllGames()
        {
            return await _context.Games.ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetAllReviews()
        {
            return await _context.Reviews.ToListAsync();
        }

        public async Task DeleteComment(Comment comment)
        {
            var itemToRemove = await _context.Comments.SingleOrDefaultAsync(c => c.CommentId == comment.CommentId);
            if (itemToRemove != null)
            {
                _context.Comments.Remove(itemToRemove);
                //await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteGame(Game game)
        {
            var itemToRemove = await _context.Games.SingleOrDefaultAsync(c => c.Id == game.Id);
            if (itemToRemove != null)
            {
                _context.Games.Remove(itemToRemove);
                //await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteReview(Review review)
        {
            var itemToRemove = await _context.Reviews.SingleOrDefaultAsync(c => c.Id == review.Id);
            if (itemToRemove != null)
            {
                _context.Reviews.Remove(itemToRemove);
                //await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateReview(Review review)
        {
            var itemToUpdate = await _context.Reviews.SingleOrDefaultAsync(r => r.Id == review.Id);

            if (itemToUpdate != null)
            {
                itemToUpdate.ReviewTitle = review.ReviewTitle;
                itemToUpdate.Author = review.Author;
                itemToUpdate.VideoUrl = review.VideoUrl;
                itemToUpdate.Introduction = review.Introduction;
                itemToUpdate.Body = review.Body;
                itemToUpdate.Conclusion = review.Conclusion;
                itemToUpdate.DatePosted = review.DatePosted;

                //itemToUpdate.Game = review.Game;
                //itemToUpdate.Comments = review.Comments;
                //May or may not be needed
            }
        }

        public async Task UpdateGame(Game game)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateComment(Comment comment)
        {
            throw new NotImplementedException();
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
