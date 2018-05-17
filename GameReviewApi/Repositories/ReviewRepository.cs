using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameReviewApi.Data;
using GameReviewApi.Models;

namespace GameReviewApi.Repositories
{
    public class ReviewRepository
    {
        private ReviewContext _context;

        public ReviewRepository(ReviewContext context)
        {
            _context = context;
        }

        public async Task<List<Review>> GetAllReviewsAsync()
        {
            return await _context.Reviews.AsNoTracking().ToListAsync();
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

        public async Task<Review> GetReviewCommentsAsync()
        {
            return null;
        }

        public async Task<Review> EditReviewAsync()
        {
            return null;
        }

    }
}
