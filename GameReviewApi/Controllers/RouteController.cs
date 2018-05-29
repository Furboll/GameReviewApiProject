using GameReviewApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameReviewApi.Controllers
{
    [Route("api")]
    public class RouteController : Controller
    {
        private IUrlHelper _urlHelper;

        public RouteController(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        [HttpGet(Name = "GetRoot")]
        public IActionResult GetRoot([FromHeader(Name = "Accept")] string mediaType)
        {
            if (mediaType == "application/vnd.gamextime.hateoas+json")
            {
                var links = new List<LinkDto>();

                links.Add(
                    new LinkDto(_urlHelper.Link("GetRoot", new { }),
                    "self",
                    "GET"));

                links.Add(
                    new LinkDto(_urlHelper.Link("GetAuthors", new { }),
                    "self",
                    "GET"));

                links.Add(
                    new LinkDto(_urlHelper.Link("CreateAuthors", new { }),
                    "self",
                    "POST"));

                return Ok(links);
            }
            else
            {
                return NoContent();
            }
        }
    }
}
