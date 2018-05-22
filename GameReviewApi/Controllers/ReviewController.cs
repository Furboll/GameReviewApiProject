using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameReviewApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GameReviewApi.Models;
using AutoMapper;
using GameReviewApi.Entities;

namespace GameReviewApi.Controllers
{
    [Produces("application/json")]
    [Route("api/reviews")]
    public class ReviewController : Controller
    {
        public IReviewRepository _reviewRepository;

        public ReviewController(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetReviews()
        {
            var reviewsFromDb = await _reviewRepository.GetAllReviews();

            var reviews = Mapper.Map<IEnumerable<ReviewDto>>(reviewsFromDb);

            //return new JsonResult(reviews);
            return Ok(reviews);
        }

        [HttpGet("{id}", Name ="GetReview")]
        public async Task<IActionResult> GetReview(int id)
        {
            var reviewFromDb = await _reviewRepository.GetReviewById(id);

            if (reviewFromDb == null)
            {
                return NotFound();
            }

            var reviewDTO = Mapper.Map<ReviewDto>(reviewFromDb);
            return Ok(reviewDTO);

            //return Ok(reviewFromDb); //replace this with above code
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] ReviewForCreationDto review)
        {
            if (review == null)
            {
                return BadRequest();
            }

            var reviewEntity = Mapper.Map<Review>(review);

            await _reviewRepository.AddReview(reviewEntity);

            if (!await _reviewRepository.Save())
            {
                throw new Exception("Creating a review failed on save.");
            }

            var reviewToReturn = Mapper.Map<ReviewDto>(reviewEntity);

            return CreatedAtRoute("GetReview",
                new { id = reviewToReturn.Id }, reviewToReturn);

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var reviewFromRepo = await _reviewRepository.GetReviewById(id);

            if (reviewFromRepo == null)
            {
                return NotFound();
            }

            await _reviewRepository.DeleteReview(reviewFromRepo);

            if (!await _reviewRepository.Save())
            {
                throw new Exception($"Deleting review {id} failed on save.");
            }

            return NoContent();
        }
    }
}