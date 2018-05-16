using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameReviewApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Review")]
    public class ReviewController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetAllReviews()
        {
        }
    }
}