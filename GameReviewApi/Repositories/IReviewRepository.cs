using GameReviewApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameReviewApi.Repositories
{
    public interface IReviewRepository
    {
        Task CreateReview(Review review);
        Task<Review> EditReviewAsync(int id);
        Task<Review> FindReviewByGameAsync(string name);
        Task<Review> FindReviewByIdAsync(int id);
        Task<List<Review>> GetAllReviewsAsync();
        Task<Review> GetLatestReview();
    }
}
