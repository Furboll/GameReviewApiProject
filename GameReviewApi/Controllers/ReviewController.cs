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
using Microsoft.AspNetCore.JsonPatch;
using GameReviewApi.Helpers;
using GameReviewApi.Services;

namespace GameReviewApi.Controllers
{
    [Produces("application/json")]
    [Route("api/reviews")]
    public class ReviewController : Controller
    {
        private IReviewRepository _reviewRepository;
        private IUrlHelper _urlHelper;
        private IPropertyMappingService _propertyMappingService;
        private ITypeHelperService _typeHelperService;

        public ReviewController(IReviewRepository reviewRepository, IUrlHelper urlHelper, IPropertyMappingService propertyMappingService, 
            ITypeHelperService typeHelperService)
        {
            _reviewRepository = reviewRepository;
            _urlHelper = urlHelper;
            _propertyMappingService = propertyMappingService;
            _typeHelperService = typeHelperService;
        }

        [HttpGet(Name = "GetReviews")]
        public async Task<IActionResult> GetReviews(ReviewResourceParameters reviewResourceParameters, [FromHeader(Name ="Accept")] string mediaType)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<ReviewDto,Review>(reviewResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<ReviewDto>(reviewResourceParameters.Fields))
            {
                return BadRequest();
            }

            var reviewsFromDb = await _reviewRepository.GetAllReviews(reviewResourceParameters);

            var reviews = Mapper.Map<IEnumerable<ReviewDto>>(reviewsFromDb);

            if (mediaType == "application/vnd.gamextime.hateoas+json")
            {
                var paginationMetadata = new
                {
                    totalCount = reviewsFromDb.TotalCount,
                    pageSize = reviewsFromDb.PageSize,
                    currentPage = reviewsFromDb.CurrentPage,
                    totalPages = reviewsFromDb.TotalPages,
                };

                Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                var links = CreateLinksForReviews(reviewResourceParameters, reviewsFromDb.HasNext, reviewsFromDb.HasPrevious);

                var shapedReviews = reviews.ShapeData(reviewResourceParameters.Fields);

                var shapedReviewsWithLinks = shapedReviews.Select(review =>
                {
                    var reviewAsDictionary = review as IDictionary<string, object>;
                    var reviewLinks = CreateLinksForReview((int)reviewAsDictionary["Id"], reviewResourceParameters.Fields);

                    reviewAsDictionary.Add("links", reviewLinks);

                    return reviewAsDictionary;
                });

                var linkedCollectionResource = new
                {
                    value = shapedReviewsWithLinks,
                    links = links
                };

                return Ok(linkedCollectionResource);
            }
            else
            {
                var previousPageLink = reviewsFromDb.HasPrevious ?
                    CreateReviewResourceUri(reviewResourceParameters,
                    ResourceUriType.PreviousPage) : null;

                var nextPageLink = reviewsFromDb.HasNext ?
                    CreateReviewResourceUri(reviewResourceParameters,
                    ResourceUriType.NextPage) : null;

                var paginationMetadata = new
                {
                    previousPageLink = previousPageLink,
                    nextPageLink = nextPageLink,
                    totalCount = reviewsFromDb.TotalCount,
                    pageSize = reviewsFromDb.PageSize,
                    currentPage = reviewsFromDb.CurrentPage,
                    totalPages = reviewsFromDb.TotalPages
                };

                Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

                return Ok(reviews.ShapeData(reviewResourceParameters.Fields));
            }
            //return Ok(reviews);
            //return Ok(reviews.ShapeData(reviewResourceParameters.Fields));
        }

        private string CreateReviewResourceUri(ReviewResourceParameters reviewResourceParameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetReviews",
                        new
                        {
                            fields = reviewResourceParameters.Fields,
                            orderBy = reviewResourceParameters.OrderBy,
                            searchQuery = reviewResourceParameters.SearchQuery,
                            gameTitle = reviewResourceParameters.GameTitle,
                            reviewTitle = reviewResourceParameters.ReviewTitle,
                            pageNumber = reviewResourceParameters.PageNumber -1,
                            pageSize = reviewResourceParameters.PageSize
                        });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetReviews",
                        new
                        {
                            fields = reviewResourceParameters.Fields,
                            orderBy = reviewResourceParameters.OrderBy,
                            searchQuery = reviewResourceParameters.SearchQuery,
                            gameTitle = reviewResourceParameters.GameTitle,
                            reviewTitle = reviewResourceParameters.ReviewTitle,
                            pageNumber = reviewResourceParameters.PageNumber + 1,
                            pageSize = reviewResourceParameters.PageSize
                        });
                case ResourceUriType.Current:
                    default:
                    return _urlHelper.Link("GetReviews",
                        new
                        {
                            fields = reviewResourceParameters.Fields,
                            orderBy = reviewResourceParameters.OrderBy,
                            searchQuery = reviewResourceParameters.SearchQuery,
                            gameTitle = reviewResourceParameters.GameTitle,
                            reviewTitle = reviewResourceParameters.ReviewTitle,
                            pageNumber = reviewResourceParameters.PageNumber,
                            pageSize = reviewResourceParameters.PageSize
                        });
            }
        }

        [HttpGet("{id}", Name ="GetReview")]
        public async Task<IActionResult> GetReview(int id,[FromQuery] string fields, [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!_typeHelperService.TypeHasProperties<ReviewDto>(fields))
            {
                return BadRequest();
            }

            var reviewFromDb = await _reviewRepository.GetReviewById(id);

            if (reviewFromDb == null)
            {
                return NotFound();
            }
            
            var review = Mapper.Map<ReviewDto>(reviewFromDb);

            if (mediaType == "application/vnd.gamextime.hateoas+json")
            {
                var links = CreateLinksForReview(id, fields);

                var linkedResourceToReturn = review.ShapeData(fields)
                    as IDictionary<string, object>;

                linkedResourceToReturn.Add("links", links);

                return Ok(linkedResourceToReturn);
            }
            else
            {
                return Ok(review.ShapeData(fields));
            }

            //var links = CreateLinksForReview(id, fields);
            //var linkedResourceToReturn = review.ShapeData(fields)
            //    as IDictionary<string, object>;
            //linkedResourceToReturn.Add("links", links);
            //return Ok(linkedResourceToReturn);

            //return Ok(review.ShapeData(fields));
            //return Ok(reviewFromDb); //replace this with above code
        }

        [HttpPost(Name = "CreateReview")]
        public async Task<IActionResult> CreateReview([FromBody] ReviewForCreationDto review, [FromHeader(Name = "Accept")] string mediaType)
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

            if (mediaType == "application/vnd.gamextime.hateoas+json")
            {
                var links = CreateLinksForReview(reviewToReturn.Id, null);

                var linkedResourceToReturn = reviewToReturn.ShapeData(null)
                    as IDictionary<string, object>;

                linkedResourceToReturn.Add("links", links);

                return CreatedAtRoute("GetReview",
                    new { id = linkedResourceToReturn["Id"] }, linkedResourceToReturn);
            }
            else
            {
                return CreatedAtRoute("GetReview",
                    new { id = reviewToReturn.Id},
                    reviewToReturn);
            }

            //var links = CreateLinksForReview(reviewToReturn.Id, null);
            //var linkedResourceToReturn = reviewToReturn.ShapeData(null)
            //    as IDictionary<string, object>;
            //linkedResourceToReturn.Add("links", links);
            //return CreatedAtRoute("GetReview",
            //    new { id = linkedResourceToReturn["Id"] }, linkedResourceToReturn);

        }

        [HttpDelete("{id}", Name = "DeleteReview")]
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

        [HttpPut("{id}", Name = "UpdateReview")]
        public async Task<IActionResult> UpdateReview(int reviewId, [FromBody] ReviewForUpdateDto review)
        {
            if (review == null)
            {
                return BadRequest();
            }

            if (!await _reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }

            var reviewForGameFromRepo = await _reviewRepository.GetReviewById(reviewId);
            if (reviewForGameFromRepo == null)
            {
                return NotFound();
            }

            Mapper.Map(review, reviewForGameFromRepo);

            await _reviewRepository.UpdateReview(reviewForGameFromRepo);

            if (!await _reviewRepository.Save())
            {
                throw new Exception($"Updating game {reviewId} failed on save!");
            }

            return NoContent();

        }

        [HttpPatch("{id}", Name = "PartiallyUpdateReview")]
        public async Task<IActionResult> PartiallyUpdateReview(int reviewId,
            [FromBody] JsonPatchDocument<ReviewForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            if (!await _reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }

            var reviewFromRepo = await _reviewRepository.GetReviewById(reviewId);

            if (reviewFromRepo == null)
            {
                return NotFound();
            }

            var reviewToPatch = Mapper.Map<ReviewForUpdateDto>(reviewFromRepo);

            patchDoc.ApplyTo(reviewToPatch);

            Mapper.Map(reviewToPatch, reviewFromRepo);

            await _reviewRepository.UpdateReview(reviewFromRepo);

            if (!await _reviewRepository.Save())
            {
                throw new Exception($"Failed to update review {reviewId}, try again.");
            }

            return NoContent();
        }

        private IEnumerable<LinkDto> CreateLinksForReview(int id, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                    new LinkDto(_urlHelper.Link("GetReview", new { id = id }),
                    "self",
                    "GET"));
            }
            else
            {
                links.Add(
                    new LinkDto(_urlHelper.Link("GetReview", new { id = id, fields = fields }),
                    "self",
                    "GET"));
            }

            links.Add(
                new LinkDto(_urlHelper.Link("DeleteReview", new { id = id }),
                "delete_review",
                "DELETE"));

            links.Add(
                new LinkDto(_urlHelper.Link("CreateGameForReview", new { reviewId = id }),
                "create_game_for_review",
                "POST"));

            links.Add(
                new LinkDto(_urlHelper.Link("GetGameForReview", new { reviewId = id }),
                "game_review",
                "GET"));

            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForReviews(ReviewResourceParameters reviewResourceParameters,
            bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>();

            links.Add(
                new LinkDto(CreateReviewResourceUri(reviewResourceParameters, ResourceUriType.Current),
                "self", "GET"));

            if (hasNext)
            {
                links.Add(
                    new LinkDto(CreateReviewResourceUri(reviewResourceParameters,
                    ResourceUriType.NextPage),
                    "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateReviewResourceUri(reviewResourceParameters,
                    ResourceUriType.PreviousPage),
                    "previousPage", "GET"));
            }

            return links;
        }
    }
}