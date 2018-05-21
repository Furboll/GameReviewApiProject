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
            await _context.SaveChangesAsync();
        }

        public async Task AddGame(Game game)
        {
            await _context.Games.AddAsync(game);
            await _context.SaveChangesAsync();
        }

        public async Task AddComment(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<Comment> FindCommentById(int Id)
        {
            return await _context.Comments
                .Where(c => c.CommentId == Id)
                .SingleOrDefaultAsync();
        }

        public async Task<Comment> FindCommentsByReviewId()
        {
            throw new NotImplementedException();
        }

        public async Task<Game> FindGameById(int Id)
        {
            return await _context.Games
                .Where(g => g.Id == Id)
                .SingleOrDefaultAsync();
        }

        public async Task<Game> FindGameByReviewId()
        {
            throw new NotImplementedException();
        }

        public async Task<Review> FindReviewByGameId()
        {
            throw new NotImplementedException();
        }

        public async Task<Review> FindReviewById(int Id)
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

        public async Task RemoveComment(int id)
        {
            var itemToRemove = await _context.Comments.SingleOrDefaultAsync(c => c.CommentId == id);
            if (itemToRemove != null)
            {
                _context.Comments.Remove(itemToRemove);
                //await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveGame(int id)
        {
            var itemToRemove = await _context.Games.SingleOrDefaultAsync(c => c.Id == id);
            if (itemToRemove != null)
            {
                _context.Games.Remove(itemToRemove);
                //await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveReview(int id)
        {
            var itemToRemove = await _context.Reviews.SingleOrDefaultAsync(c => c.Id == id);
            if (itemToRemove != null)
            {
                _context.Reviews.Remove(itemToRemove);
                //await _context.SaveChangesAsync();
            }
        }

        public async Task Update(Review review)
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

        public async Task Update(Game game)
        {
            throw new NotImplementedException();
        }

        public async Task Update(Comment comment)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Save()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
