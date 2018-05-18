using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameReviewApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GameReviewApi.Models;

namespace GameReviewApi.Controllers
{
    [Produces("application/json")]
    [Route("api/review")]
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
            var reviews = await _reviewRepository.GetReviews();
            return Ok(reviews);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReview(int id)
        {
            var review = await _reviewRepository.GetReview(id);

            if (review == null)
            {
                return NotFound();
            }

            //var reviewDTO = Mapper.Map<AuthorDto>(review);
            //retirn Ok(reviewDTO);

            return Ok(review);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] ReviewForCreationDto review)
        {
            if (review == null)
            {
                return BadRequest();
            }

        }
    }
}