using GameReviewApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameReviewApi.Repositories
{
    public interface IReviewRepository
    {
        Task AddReview(Review review);
        Task<IEnumerable<Review>> GetAllReviews();
        Task<Review> FindReviewById(int Id);
        Task<Review> FindReviewByGameId();
        Task RemoveReview(int id);
        Task Update(Review review);

        Task AddGame(Game game);
        Task<IEnumerable<Game>> GetAllGames();
        Task<Game> FindGameById(int Id);
        Task<Game> FindGameByReviewId();
        Task RemoveGame(int id);
        Task Update(Game game);

        Task AddComment(Comment comment);
        Task<IEnumerable<Comment>> GetAllComments();
        Task<Comment> FindCommentById(int Id);
        Task<Comment> FindCommentsByReviewId();
        Task RemoveComment(int id);
        Task Update(Comment comment);

        Task<bool> Save();
        //Task<bool> ReviewExists(int reviewId);
        //Task<Review> EditReviewAsync(int id);
        //Task<Review> GetReviewByGameIdAsync(int id);
        //Task<Review> GetReviewByIdAsync(int id);
        //Task CreateReview(Review review);
        //Task<List<Review>> GetAllReviewsAsync();
        //Task<Review> GetLatestReview();
    }
}
