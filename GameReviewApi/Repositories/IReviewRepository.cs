using GameReviewApi.Entities;
using GameReviewApi.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameReviewApi.Repositories
{
    public interface IReviewRepository
    {
        Task AddReview(Review review);
        Task<PagedList<Review>> GetAllReviews(ReviewResourceParameters reviewResourceParameters);
        Task<IEnumerable<Review>> GetAllReviews(IEnumerable<int> reviewIds);
        Task<Review> GetReviewById(int reviewId);
        Task<Review> GetReviewByGameId(int gameId, int reviewId);
        Task DeleteReview(Review review);
        Task UpdateReview(Review review);

        Task AddGame(Game game);
        Task<IEnumerable<Game>> GetAllGames();
        Task<Game> GetGameById(int gameId);
        Task<Game> GetGameByReviewId(int gameId, int reviewId);
        Task DeleteGame(Game game);
        Task UpdateGame(Game game);

        Task AddComment(Comment comment);
        Task<IEnumerable<Comment>> GetAllComments();
        Task<Comment> GetCommentById(int commentId);
        Task<Comment> GetCommentsByReviewId(int reviewId, int commentId);
        Task DeleteComment(Comment comment);
        Task UpdateComment(Comment comment);

        Task<bool> ReviewExists(int reviewId);
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
