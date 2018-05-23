using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GameReviewApi.Models;
using GameReviewApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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
        public async Task<IActionResult> GetReviewedGames(int reviewId)
        {
            if (!await _reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }

            var gameForReviewFromDb = _reviewRepository.GetReviewByGameId(gameId, reviewId);

            var gameForReview = Mapper.Map<IEnumerable<GameDto>>(gameForReviewFromDb);

            return Ok(gameForReview);
        }

        [HttpGet("{id}", Name = "GetReviewForGame")]
        public async Task<IActionResult> GetReviewForGame(int reviewId, int gameId)
        {
            if (!await _reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }

            var gameReviewFromRepo = await _reviewRepository.GetReviewByGameId(gameId, reviewId);
            if (gameReviewFromRepo == null)
            {
                return NotFound();
            }

            var reviewForGame = Mapper.Map<GameDto>(gameReviewFromRepo); //<-- needs to change most prob... cept I forgot to what X_X

            return Ok(reviewForGame);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int reviewId, int gameId)
        {
            if (!await _reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }

            var gameForReviewFromRepo = await _reviewRepository.GetGameByReviewId(gameId, reviewId);
            if (gameForReviewFromRepo == null)
            {
                return NotFound();
            }

            await _reviewRepository.DeleteGame(gameForReviewFromRepo);

            if (!await _reviewRepository.Save())
            {

                throw new Exception($"Deleting game {gameId} for review {reviewId} failed on save");
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGame(int gameId, [FromBody] GameForUpdateDto game)
        {
            if (game == null)
            {
                return BadRequest();
            }

            if (!await _reviewRepository.ReviewExists(gameId))
            {
                return NotFound();
            }

            var gameForReviewFromRepo = await _reviewRepository.GetGameById(gameId);
            if (gameForReviewFromRepo == null)
            {
                return NotFound();
            }

            Mapper.Map(game, gameForReviewFromRepo);

            await _reviewRepository.UpdateGame(gameForReviewFromRepo);

            if (!await _reviewRepository.Save())
            {
                throw new Exception($"Updating game {gameId} failed on save!");
            }

            return NoContent();

        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateGame(int reviewId, int gameId, 
            [FromBody] JsonPatchDocument<GameForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            if (!await _reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }

            var gameForReviewFromRepo = await _reviewRepository.GetGameByReviewId(gameId, reviewId);

            if (gameForReviewFromRepo == null)
            {
                return NotFound();
            }

            var gameToPatch = Mapper.Map<GameForUpdateDto>(gameForReviewFromRepo);

            patchDoc.ApplyTo(gameToPatch);

            Mapper.Map(gameToPatch, gameForReviewFromRepo);

            await _reviewRepository.UpdateGame(gameForReviewFromRepo);

            if (!await _reviewRepository.Save())
            {
                throw new Exception($"Failed to update game {gameId}, try again.");
            }

            return NoContent();
        }

    }
}