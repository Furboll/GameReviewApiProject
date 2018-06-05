using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GameReviewApi.Entities;
using GameReviewApi.Helpers;
using GameReviewApi.Models;
using GameReviewApi.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameReviewApi.Controllers
{
    [Route("api/reviews/{reviewId}/comments")]
    public class CommentsController : Controller
    {
        private IReviewRepository _reviewRepository;
        private ILogger<CommentsController> _logger;
        private IUrlHelper _urlHelper;

        public CommentsController(IReviewRepository reviewRepository, ILogger<CommentsController> logger, IUrlHelper urlHelper)
        {
            _reviewRepository = reviewRepository;
            _logger = logger;
            _urlHelper = urlHelper;
        }

        [HttpGet(Name = "GetCommentsForReview")]
        public async Task<IActionResult> GetCommentsForReview(int reviewId)
        {
            if (!await _reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }

            var commentsForReviewFromRepo = await _reviewRepository.GetCommentsForReview(reviewId);

            var commentsForReview = Mapper.Map<IEnumerable<CommentDto>>(commentsForReviewFromRepo);

            commentsForReview = commentsForReview.Select(comment =>
            {
                comment = CreateLinksForComment(comment);
                return comment;
            });

            var wrapper = new LinkedCollectionResourceWrapperDto<CommentDto>(commentsForReview);

            return Ok(CreateLinksForComments(wrapper));
        }

        [HttpGet("{id}", Name = "GetCommentForReview")]
        public async Task<IActionResult> GetCommentForReview(int reviewId, int id)
        {
            if (!await _reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }

            var reviewCommentFromRepo = await _reviewRepository.GetCommentForReview(reviewId, id);

            if (reviewCommentFromRepo == null)
            {
                return NotFound();
            }

            var commentForReview = Mapper.Map<CommentDto>(reviewCommentFromRepo);
            return Ok(CreateLinksForComment(commentForReview));
        }

        [HttpPost(Name ="CreateCommentForReview")]
        public async Task<IActionResult> CreateCommentForReview(int reviewId, [FromBody]CommentForCreationDto comment)
        {
            if (comment == null)
            {
                return BadRequest();
            }

            if (comment.CommentContent == comment.Author)
            {
                ModelState.AddModelError(nameof(CommentForCreationDto), "The title should not have developer or publisher in it");
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            if (!await _reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }

            var commentEntity = Mapper.Map<Comment>(comment);

            await _reviewRepository.AddComment(reviewId, commentEntity);

            if (!await _reviewRepository.Save())
            {
                throw new Exception($"Creating a comment for review {reviewId} failed on save");
            }

            var commentToReturn = Mapper.Map<CommentDto>(commentEntity);

            return CreatedAtRoute("GetCommentForGame",
                new { reviewId = reviewId, id = commentToReturn.Id }, CreateLinksForComment(commentToReturn));
        }

        [HttpDelete("{id}", Name = "DeleteComment")]
        public async Task<IActionResult> DeleteComment(int reviewId, int id)
        {
            if (!await _reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }

            var commentForReviewFromRepo = await _reviewRepository.GetCommentForReview(reviewId, id);

            if (commentForReviewFromRepo == null)
            {
                return NotFound();
            }

            await _reviewRepository.DeleteComment(commentForReviewFromRepo);

            if (!await _reviewRepository.Save())
            {
                throw new Exception($"Deleting comment {id} for review {reviewId} failed on save");
            }

            _logger.LogInformation(100, $"Comment {id} for review {reviewId} was deleted");

            return NoContent();
        }

        [HttpPatch("{id}", Name = "PartiallyUpdateComment")]
        public async Task<IActionResult> PartiallyUpdateComment(int reviewId, int id, 
            [FromBody] JsonPatchDocument<CommentForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            if (!await _reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }

            var commentForReviewFromRepo = await _reviewRepository.GetCommentForReview(reviewId, id);

            if (commentForReviewFromRepo == null)
            {
                return NotFound();
            }

            var commentToPatch = Mapper.Map<CommentForUpdateDto>(commentForReviewFromRepo);

            patchDoc.ApplyTo(commentToPatch, ModelState);

            if (commentToPatch.CommentContent == commentToPatch.Author)
            {
                ModelState.AddModelError(nameof(CommentForUpdateDto), "The provided content should be different from the Author name.");
            }

            TryValidateModel(commentToPatch);

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            Mapper.Map(commentToPatch, commentForReviewFromRepo);

            await _reviewRepository.UpdateComment(commentForReviewFromRepo);

            if (!await _reviewRepository.Save())
            {
                throw new Exception($"Failed to update comment {id}, try again.");
            }

            return NoContent();
        }

        private CommentDto CreateLinksForComment(CommentDto comment)
        {
            comment.Links.Add(new LinkDto(_urlHelper.Link("GetCommentForReview",
                new { id = comment.Id }),
                "self",
                "GET"));

            comment.Links.Add(
                new LinkDto(_urlHelper.Link("DeleteComment",
                new { id = comment.Id }),
                "delete_comment",
                "DELETE"));

            //comment.Links.Add(
            //    new LinkDto(_urlHelper.Link("UpdateComment",
            //    new { id = comment.Id }),
            //    "update_game",
            //    "PUT"));

            comment.Links.Add(
                new LinkDto(_urlHelper.Link("PartiallyUpdateComment",
                new { id = comment.Id }),
                "partially_update_comment",
                "PATCH"));

            return comment;
        }

        private LinkedCollectionResourceWrapperDto<CommentDto> CreateLinksForComments(LinkedCollectionResourceWrapperDto<CommentDto> commentWrapper)
        {
            commentWrapper.Links.Add(
                new LinkDto(_urlHelper.Link("GetCommentsForReview", new { }),
                "self",
                "GET"));

            return commentWrapper;
        }
    }
}
