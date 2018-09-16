using System;
using System.Threading.Tasks;
using GP.Microservices.Api.Models;
using GP.Microservices.Common.Authentication;
using GP.Microservices.Common.Messages.Remarks.Commands;
using GP.Microservices.Common.Messages.Remarks.Queries;
using GP.Microservices.Common.ServiceClients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GP.Microservices.Api.Controllers
{
    /// <summary>
    /// Remark resource endpoints
    /// </summary>
    [Authorize]
    [Route("api/remarks")]
    public class RemarksController : ControllerBase
    {
        private readonly IRemarkServiceClient _remarkService;
        private readonly IStorageServiceClient _storageService;

        public RemarksController(
            IRemarkServiceClient remarkService,
            IStorageServiceClient storageService)
        {
            _remarkService = remarkService;
            _storageService = storageService;
        }

        /// <summary>
        /// Browse remarks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Browse()
        {
            var username = HttpContext.User.Identity.Name;

            var query = new BrowseRemarks
            {
                Latitude = 19,
                Longitude = 19,
                Radius = 10000
            };

            var result = await _storageService
                .BrowseRemarksAsync(query)
                .OrFailAsync();


            return Ok(result);
        }

        /// <summary>
        /// Get remark details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _storageService
                .GetRemarkAsync(id)
                .OrFailAsync();

            return Ok(result);
        }

        /// <summary>
        /// Create remark
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRemarkRequest request)
        {
            var userId = HttpContext.GetUserId();

            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var command = new CreateRemark
            {
                Name = request.Name,
                CategoryId = request.CategoryId,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                Description = request.Description,
                UserId = userId
            };

            var result = await _remarkService
                .CreateRemarkAsync(command)
                .OrFailAsync();

            return Ok(result);
        }

        /// <summary>
        /// Resolve remark
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id:guid}/resolve")]
        public async Task<IActionResult> Resolve(Guid id)
        {
            var userId = HttpContext.GetUserId();

            var command = new ResolveRemark
            {
                RemarkId = id,
                UserId = userId
            };

            var result = await _remarkService
                .ResolveRemarkAsync(command)
                .OrFailAsync();

            return Ok(result);
        }

        /// <summary>
        /// Cancel remark
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id:guid}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var userId = HttpContext.GetUserId();

            var command = new CancelRemark
            {
                RemarkId = id,
                UserId = userId
            };

            var result = await _remarkService
                .CancelRemarkAsync(command)
                .OrFailAsync();

            return Ok(result);
        }

        /// <summary>
        /// Delete remark
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = HttpContext.GetUserId();

            var remark = await _storageService
                .GetRemarkAsync(id)
                .OrFailAsync();

            if (remark.Author != id)
            {
                return BadRequest();
            }

            var command = new DeleteRemark
            {
                RemarkId = id,
                UserId = userId
            };

            var result = await _remarkService
                .DeleteRemarkAsync(command)
                .OrFailAsync();

            return Ok(result);
        }

        /// <summary>
        /// Add image
        /// </summary>
        /// <param name="remarkId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("{remarkId:guid}/images")]
        public async Task<IActionResult> AddImage(Guid remarkId, [FromBody] AddImageRequest request)
        {
            var userId = HttpContext.GetUserId();

            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var command = new AddImage
            {
                RemarkId = remarkId,
                UserId = userId,
                Base64 = request.Base64,
                Name = request.Name ?? Guid.NewGuid().ToString(),
                Order = request.Order,
                ActivityId = request.ActivityId
            };

            var result = await _remarkService
                .AddImageAsync(command)
                .OrFailAsync();

            return Ok(result);
        }

        /// <summary>
        /// Remove image
        /// </summary>
        /// <param name="remarkId"></param>
        /// <param name="imageId"></param>
        /// <returns></returns>
        [HttpDelete("{remarkId:guid}/images/{imageId:guid}")]
        public async Task<IActionResult> RemoveImage(Guid remarkId, Guid imageId)
        {
            var userId = HttpContext.GetUserId();

            var command = new RemoveImage
            {
                ImageId = imageId,
                RemarkId = remarkId,
                UserId = userId
            };

            var result = await _remarkService
                .RemoveImageAsync(command)
                .OrFailAsync();

            return Ok(result);
        }

        /// <summary>
        /// Add comment
        /// </summary>
        /// <param name="remarkId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("{remarkId:guid}/comments")]
        public async Task<IActionResult> AddComment(Guid remarkId, [FromBody] AddCommentRequest request)
        {
            var userId = HttpContext.GetUserId();

            var command = new AddComment
            {
                RemarkId = remarkId,
                Text = request.Text,
                UserId = userId
            };

            var result = await _remarkService
                .AddCommentAsync(command)
                .OrFailAsync();

            return Ok(result);
        }

        /// <summary>
        /// Remove comment
        /// </summary>
        /// <param name="remarkId"></param>
        /// <param name="commentId"></param>
        /// <returns></returns>
        [HttpPost("{remarkId:guid}/comments/{commentId:guid}")]
        public async Task<IActionResult> RemoveComment(Guid remarkId, Guid commentId)
        {
            var userId = HttpContext.GetUserId();

            var command = new RemoveComment
            {
                RemarkId = remarkId,
                UserId = userId,
                CommentId = commentId
            };

            var result = await _remarkService
                .RemoveCommentAsync(command)
                .OrFailAsync();

            return Ok(result);
        }

        /// <summary>
        /// Add activity
        /// </summary>
        /// <param name="remarkId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("{remarkId:guid}/activities")]
        public async Task<IActionResult> AddActivity(Guid remarkId, [FromBody] AddActivityRequest request)
        {
            var userId = HttpContext.GetUserId();

            var command = new AddActivity
            {
                RemarkId = remarkId,
                UserId = userId,
                ActivityTypeId = request.ActivityTypeId,
                Date = request.Date
            };

            var result = await _remarkService
                .AddActivityAsync(command)
                .OrFailAsync();

            return Ok(result);
        }
    }
}