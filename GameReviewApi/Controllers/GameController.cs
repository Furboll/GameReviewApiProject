using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GameReviewApi.Models;
using GameReviewApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameReviewApi.Controllers
{
    [Produces("application/json")]
    [Route("api/reviews/{reviewId}/games")]
    public class GameController : Controller
    {
        public IReviewRepository _reviewRepository;

        public GameController(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        
        public async Task<IActionResult> GetGameReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }

            var gameForReviewFromDb = _reviewRepository.FindReviewByGameAsync(reviewId);

            var gameForReview = Mapper.Map<IEnumerable<GameDto>>(gameReviewFromDb);
        }
    }
}