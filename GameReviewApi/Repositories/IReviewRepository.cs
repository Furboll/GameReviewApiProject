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
        Task<PagedList<Review>> GetAllReviews(ReviewResourceParameters reviewResourceParameters);
        Task<Review> GetReviewById(int reviewId);
        //Task<IEnumerable<Review>> GetAllReviews(IEnumerable<int> reviewIds);
        //Task<Review> GetReviewByGameId(int gameId, int reviewId);
        Task AddReview(Review review);
        Task DeleteReview(Review review);
        Task UpdateReview(Review review);

        //Task<IEnumerable<Game>> GetAllGames();
        //Task<Game> GetGameById(int gameId);
        Task<Game> GetGameForReview(int reviewId, int gameId); //(int gameId, int reviewId);
        Task AddGame(int reviewId, Game game);
        Task DeleteGame(Game game);
        Task UpdateGame(Game game);

        //Task<IEnumerable<Comment>> GetAllComments();
        Task<IEnumerable<Comment>> GetCommentsForReview(int reviewId);
        //Task<Comment> GetCommentById(int commentId);
        Task<Comment> GetCommentForReview(int reviewId, int commentId);
        Task AddComment(int reviewId, Comment comment);
        Task DeleteComment(Comment comment);
        Task UpdateComment(Comment comment);

        Task<bool> ReviewExists(int reviewId);
        Task<bool> Save();
    }
}
