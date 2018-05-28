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
        public async Task<IActionResult> GetReviews(ReviewResourceParameters reviewResourceParameters)
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

            var previousPageLink = reviewsFromDb.HasPrevious ?
                CreateReviewResourceUri(reviewResourceParameters,
                ResourceUriType.PreviousPage) : null;

            var nextPageLink = reviewsFromDb.HasNext ?
                CreateReviewResourceUri(reviewResourceParameters,
                ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                totalCount = reviewsFromDb.TotalCount,
                pageSize = reviewsFromDb.PageSize,
                currentPage = reviewsFromDb.CurrentPage,
                totalPages = reviewsFromDb.TotalPages,
                previousPageLink = previousPageLink,
                nextPageLink = nextPageLink
            };

            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            var reviews = Mapper.Map<IEnumerable<ReviewDto>>(reviewsFromDb);
            //return Ok(reviews);
            return Ok(reviews.ShapeData(reviewResourceParameters.Fields));
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
        public async Task<IActionResult> GetReview(int id,[FromQuery] string fields)
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
            
            var reviewDTO = Mapper.Map<ReviewDto>(reviewFromDb);
            return Ok(reviewDTO.ShapeData(fields));

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

        [HttpPut("{id}")]
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

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateReview(int reviewId, int gameId,
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

            var reviewForGameFromRepo = await _reviewRepository.GetReviewByGameId(gameId, reviewId);

            if (reviewForGameFromRepo == null)
            {
                return NotFound();
            }

            var reviewToPatch = Mapper.Map<ReviewForUpdateDto>(reviewForGameFromRepo);

            patchDoc.ApplyTo(reviewToPatch);

            Mapper.Map(reviewToPatch, reviewForGameFromRepo);

            await _reviewRepository.UpdateReview(reviewForGameFromRepo);

            if (!await _reviewRepository.Save())
            {
                throw new Exception($"Failed to update review {reviewId}, try again.");
            }

            return NoContent();
        }
    }
}