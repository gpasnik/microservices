using System;
using System.Threading.Tasks;
using GP.Microservices.Api.Models;
using GP.Microservices.Common.Messages.Remarks.Commands;
using GP.Microservices.Common.Messages.Remarks.Queries;
using GP.Microservices.Common.ServiceClients;
using Microsoft.AspNetCore.Mvc;

namespace GP.Microservices.Api.Controllers
{
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

        [HttpGet]
        public async Task<IActionResult> Browse()
        {
            //TODO get username from token and load default settings
            var username = "get username from token";

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

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _storageService
                .GetRemarkAsync(id)
                .OrFailAsync();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRemarkRequest request)
        {
            //TODO get userid from token
            var userId = Guid.NewGuid();

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

        [HttpPut("{id:guid}/resolve")]
        public async Task<IActionResult> Resolve(Guid id)
        {
            //TODO get userid from token
            var userId = Guid.NewGuid();

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

        [HttpPut("{id:guid}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            //TODO get userid from token
            var userId = Guid.NewGuid();

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

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            //TODO get userid from token
            var userId = Guid.NewGuid();

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

        [HttpPost("{remarkId:guid}/images")]
        public async Task<IActionResult> AddImage(Guid remarkId, [FromBody] AddImageRequest request)
        {
            //TODO get userid from token
            var userId = Guid.NewGuid();

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

        [HttpDelete("{remarkId:guid/images/{imageId:guid}")]
        public async Task<IActionResult> RemoveImage(Guid remarkId, Guid imageId)
        {
            //TODO get userid from token
            var userId = Guid.NewGuid();

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

        [HttpPost("{remarkId:guid}/comments")]
        public async Task<IActionResult> AddComment(Guid remarkId, [FromBody] AddCommentRequest request)
        {
            //TODO get userid from token
            var userId = Guid.NewGuid();

            var command = new AddComment
            {
                RemarkId = remarkId,
                Text = request.Text,
                Status = CommentStatus.Active,
                UserId = userId
            };

            var result = await _remarkService
                .AddCommentAsync(command)
                .OrFailAsync();

            return Ok(result);
        }

        [HttpPost("{remarkId:guid}/comments/{commentId:guid}")]
        public async Task<IActionResult> RemoveComment(Guid remarkId, Guid commentId)
        {
            //TODO get userid from token
            var userId = Guid.NewGuid();

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

        [HttpPost("{remarkId:guid}/activities")]
        public async Task<IActionResult> AddActivity(Guid remarkId, [FromBody] AddActivityRequest request)
        {
            //TODO get userid from token
            var userId = Guid.NewGuid();

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