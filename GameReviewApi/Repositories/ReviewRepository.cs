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
        private ReviewContext _context;

        public ReviewRepository(ReviewContext context)
        {
            _context = context;
        }

        public async Task<List<Review>> GetAllReviewsAsync()
        {
            return await _context.Reviews.ToListAsync();
        }

        public async Task<Review> FindReviewByIdAsync(int id)
        {
            return await _context.Reviews.Include(r => r.Comments).FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Review> FindReviewByGameAsync(string name)
        {
            return await _context.Reviews.Where(r => r.Game.Title == name).SingleAsync();
        }

        public async Task<Review> GetLatestReview()
        {
            return await _context.Reviews.OrderByDescending(r => r.DatePosted).FirstOrDefaultAsync();
        }

        public async Task CreateReview(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();

        }

        public async Task<Review> EditReviewAsync(int id)
        {
            return null;
        }

    }
}
