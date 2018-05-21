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

        [HttpGet()]
        public async Task<IActionResult> GetReviewsForGame(int reviewId)
        {
            if (await _reviewRepository.ReviewExists(reviewId) == false)
            {
                return NotFound();
            }

            var gameForReviewFromDb = _reviewRepository.GetReviewByGameIdAsync(reviewId);

            var gameForReview = Mapper.Map<IEnumerable<GameDto>>(gameForReviewFromDb);

            return Ok(gameForReview);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReviewForGame(int reviewId, int id)
        {
            if (await _reviewRepository.ReviewExists(reviewId) == false)
            {
                return NotFound();
            }

            var gameReviewFromRepo = await _reviewRepository.GetReviewByGameIdAsync(id);
            if (gameReviewFromRepo == null)
            {
                return NotFound();
            }

            var reviewForGame = Mapper.Map<GameDto>(gameReviewFromRepo); //<-- needs to change most prob

            return Ok(reviewForGame);
        }

    }
}