using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GameReviewApi.Entities;
using GameReviewApi.Helpers;
using GameReviewApi.Models;
using GameReviewApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameReviewApi.Controllers
{
    [Produces("application/json")]
    [Route("api/reviews/{reviewId}/games")]
    public class GameController : Controller
    {
        private IReviewRepository _reviewRepository;
        private ILogger<GameController> _logger;
        private IUrlHelper _urlHelper;

        public GameController(IReviewRepository reviewRepository, ILogger<GameController> logger, IUrlHelper urlHelper)
        {
            _reviewRepository = reviewRepository;
            _logger = logger;
            _urlHelper = urlHelper;
        }

        [HttpGet(Name = "GetReviewedGames")]
        public async Task<IActionResult> GetReviewedGames(int reviewId)
        {
            if (!await _reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }

            var gameForReviewFromDb = await _reviewRepository.GetGamesForReview(reviewId);

            if (gameForReviewFromDb == null)
            {
                return NotFound();
            }

            var gameForReview = Mapper.Map<GameDto>(gameForReviewFromDb);

            return Ok(CreateLinksForGame(gameForReview));
        }

        //[HttpGet("{gameId}", Name = "GetGameForReview")]
        //public async Task<IActionResult> GetGameForReview(int reviewId, int gameId)
        //{
        //    if (!await _reviewRepository.ReviewExists(reviewId))
        //    {
        //        return NotFound();
        //    }

        //    var gameReviewFromRepo = await _reviewRepository.GetGameForReview(reviewId, gameId);

        //    if (gameReviewFromRepo == null)
        //    {
        //        return NotFound();
        //    }

        //    var reviewForGame = Mapper.Map<GameDto>(gameReviewFromRepo);

        //    return Ok(CreateLinksForGame(reviewForGame));
        //}

        [HttpPost(Name = "CreateGameForReview")]
        public async Task<IActionResult> CreateGameForReview(int reviewId, [FromBody] GameForCreationDto game)
        {
            if (game == null)
            {
                return BadRequest();
            }

            if (game.Developer == game.Title && game.Publisher == game.Title)
            {
                ModelState.AddModelError(nameof(GameForCreationDto), "The title should not have developer or publisher in it");
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            if (!await _reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }

            var gameEntity = Mapper.Map<Game>(game);

            await _reviewRepository.AddGame(reviewId, gameEntity);

            if (!await _reviewRepository.Save())
            {
                throw new Exception($"Creating a game for review {reviewId} failed on save.");
            }

            var gameToReturn = Mapper.Map<GameDto>(gameEntity);

            return CreatedAtRoute("GetReviewedGames",
                new { reviewId = reviewId, id = gameToReturn.Id }, CreateLinksForGame(gameToReturn));
        }

        [HttpDelete("{gameId}", Name = "DeleteGame")]
        public async Task<IActionResult> DeleteGame(int reviewId, int gameId)
        {
            if (!await _reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }

            var gameForReviewFromRepo = await _reviewRepository.GetGameForReview(reviewId, gameId);

            if (gameForReviewFromRepo == null)
            {
                return NotFound();
            }

            await _reviewRepository.DeleteGame(gameForReviewFromRepo);

            if (!await _reviewRepository.Save())
            {

                throw new Exception($"Deleting game {gameId} for review {reviewId} failed on save");
            }

            _logger.LogInformation(100, $"Game {gameId} for review {reviewId} was deleted");

            return NoContent();
        }

        [HttpPut("{gameId}", Name = "UpdateGame")]
        public async Task<IActionResult> UpdateGame(int reviewId, int gameId, [FromBody] GameForUpdateDto game)
        {
            if (game == null)
            {
                return BadRequest();
            }

            if (game.Publisher == game.Title || game.Developer == game.Title)
            {
                ModelState.AddModelError(nameof(GameForUpdateDto), "The provided publisher or developer should not be the title of the game");
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            if (!await _reviewRepository.ReviewExists(gameId))
            {
                return NotFound();
            }

            var gameForReviewFromRepo = await _reviewRepository.GetGameForReview(reviewId, gameId);
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

        [HttpPatch("{gameId}", Name = "PartiallyUpdateGame")]
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

            var gameForReviewFromRepo = await _reviewRepository.GetGameForReview(reviewId, gameId);

            if (gameForReviewFromRepo == null)
            {
                return NotFound();
            }

            var gameToPatch = Mapper.Map<GameForUpdateDto>(gameForReviewFromRepo);

            patchDoc.ApplyTo(gameToPatch, ModelState);

            if (gameToPatch.Developer == gameToPatch.Title || gameToPatch.Publisher == gameToPatch.Title)
            {
                ModelState.AddModelError(nameof(GameForUpdateDto), "The provided description should be different from the title.");
            }

            TryValidateModel(gameToPatch);

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            Mapper.Map(gameToPatch, gameForReviewFromRepo);

            await _reviewRepository.UpdateGame(gameForReviewFromRepo);

            if (!await _reviewRepository.Save())
            {
                throw new Exception($"Failed to update game {gameId}, try again.");
            }

            return NoContent();
        }

        private GameDto CreateLinksForGame(GameDto game)
        {
            game.Links.Add(new LinkDto(_urlHelper.Link("GetGameForReview",
                new { id = game.Id }),
                "self",
                "GET"));

            game.Links.Add(
                new LinkDto(_urlHelper.Link("DeleteGame",
                new { id = game.Id }),
                "delete_game",
                "DELETE"));

            game.Links.Add(
                new LinkDto(_urlHelper.Link("UpdateGame",
                new { id = game.Id }),
                "update_game",
                "PUT"));

            game.Links.Add(
                new LinkDto(_urlHelper.Link("PartiallyUpdateGame",
                new { id = game.Id }),
                "partially_update_game",
                "PATCH"));

            return game;
        }

        private LinkedCollectionResourceWrapperDto<GameDto> CreateLinksForGames(LinkedCollectionResourceWrapperDto<GameDto> gameWrapper)
        {
            gameWrapper.Links.Add(
                new LinkDto(_urlHelper.Link("GetReviewdGames", new { }),
                "self",
                "GET"));

            return gameWrapper;
        }
    }
}