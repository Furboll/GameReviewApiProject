using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameReviewApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameReviewApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Review")]
    public class ReviewController : Controller
    {
        public IReviewRepository _reviewRepository;

        public ReviewController(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReviews()
        {
            
        }
    }
}