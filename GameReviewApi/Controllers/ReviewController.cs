using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameReviewApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GameReviewApi.Models;
using AutoMapper;

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
            var reviewsFromDb = await _reviewRepository.GetAllReviewsAsync();

            var reviews = Mapper.Map<IEnumerable<ReviewDto>>(reviewsFromDb);

            return new JsonResult(reviews);
            //return Ok(reviews);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReview(int id)
        {
            var reviewFromDb = await _reviewRepository.FindReviewByIdAsync(id);

            if (reviewFromDb == null)
            {
                return NotFound();
            }

            //var reviewDTO = Mapper.Map<AuthorDto>(review);
            //retirn Ok(reviewDTO);

            return Ok(reviewFromDb); //replace this with above code
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] ReviewForCreationDto review)
        {
            if (review == null)
            {
                return BadRequest();
            }

            return Ok();

        }
    }
}