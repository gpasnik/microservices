using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GP.Microservices.Common.Dto;
using GP.Microservices.Common.Messages.Remarks.Commands;
using GP.Microservices.Common.Messages.Remarks.Queries;
using GP.Microservices.Remarks.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GP.Microservices.Remarks.Controllers
{
    /// <summary>
    /// Remark resource endpoints
    /// </summary>
    [Route("api/remarks")]
    public class RemarksController : Controller
    {
        private readonly IRemarkService _remarkService;
        private readonly ICategoryService _categorySevice;
        private readonly IActivityService _activityService;

        /// <inheritdoc />
        public RemarksController(IRemarkService remarkService,
            ICategoryService categorySevice,
            IActivityService activityService)
        {
            _remarkService = remarkService;
            _categorySevice = categorySevice;
            _activityService = activityService;
        }

        /// <summary>
        /// Browse remarks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Browse()
        {
            var query = new BrowseRemarks();

            var remarks = await _remarkService.BrowseAsync(query);
            var categories = await _categorySevice.GetAsync();

            var result = remarks
                .Select(x => new RemarkDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    Author = x.AuthorId,
                    CategoryId = x.CategoryId,
                    CategoryName = categories.FirstOrDefault(y => y.Id == x.CategoryId)?.Name,
                    Status = x.Status.ToString()
                })
                .ToList();

            return Ok(result);
        }

        /// <summary>
        /// Create remark
        /// </summary>
        /// <param name="command"></param>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateRemark command)
        {
            var remark = await _remarkService.CreateAsync(command);

            var dto = new RemarkDto
            {
                Id = remark.Id
            };

            return Ok(dto);
        }

        /// <summary>
        /// Get remark details
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var remark = await _remarkService.GetAsync(id);
            var activities = await _remarkService.GetActivitiesAsync(id);
            var activityTypes = await _activityService.GetAsync();
            var comments = await _remarkService.GetCommentsAsync(id);
            var images = await _remarkService.GetImagessAsync(id);
            var category = await _categorySevice.GetAsync(remark.CategoryId);

            var dto = new RemarkDto
            {
                Id = remark.Id,
                Name = remark.Name,
                Description = remark.Description,
                Latitude = remark.Latitude,
                Longitude = remark.Longitude,
                Author = remark.AuthorId,
                CategoryId = remark.CategoryId,
                CategoryName = category.Name,
                Status = remark.Status.ToString(),
                Activities = activities?
                    .Select(x => new ActivityDto
                    {
                        Id = x.Id,
                        Name = activityTypes.FirstOrDefault(y => y.Id == x.TypeId)?.Name
                    })
                    .ToList(),
                Comments = comments
                    .Select(x => new CommentDto
                    {
                        Id = x.Id,
                        AuthorId = x.AuthorId,
                        Status = x.Status.ToString(),
                        Text = x.Text
                    })
                    .ToList(),
                Images = images
                    .Select(x => new ImageDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Url = x.Url,
                    })
                    .ToList()
            };
            return Ok(dto);
        }

        /// <summary>
        /// Resolve remark
        /// </summary>
        [HttpPut("{id:guid}/resolve")]
        public async Task<IActionResult> Resolve(Guid id, [FromBody] ResolveRemark command)
        {
            var remark = await _remarkService.ResolveAsync(command);

            var dto = new RemarkDto
            {
                Id = remark.Id
            };

            return Ok(dto);
        }

        /// <summary>
        /// Cancel remark
        /// </summary>
        [HttpPut("{id:guid}/cancel")]
        public async Task<IActionResult> Cancel(Guid id, [FromBody] CancelRemark command)
        {
            var remark = await _remarkService.CancelAsync(command);

            var dto = new RemarkDto
            {
                Id = remark.Id
            };

            return Ok(dto);
        }

        /// <summary>
        /// Delete remark
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            //todo: get userid
            var command = new DeleteRemark
            {
                RemarkId = id,
            };

            var remark = await _remarkService.DeleteAsync(command);

            var dto = new RemarkDto
            {
                Id = remark.Id
            };

            return Ok(dto);
        }

        /// <summary>
        /// Get activities
        /// </summary>
        [HttpGet("{id:guid}/activities")]
        public async Task<IActionResult> GetActivities(Guid id)
        {
            var activities = await _remarkService.GetActivitiesAsync(id);

            var result = activities
                .Select(x => new ActivityDto
                {
                    Id = x.Id,
                    Name = x.Type?.Name
                })
                .ToList();

            return Ok(result);
        }

        /// <summary>
        /// Add activity
        /// </summary>
        [HttpPost("{id:guid}/activities")]
        public async Task<IActionResult> AddActivity(Guid id, [FromBody] AddActivity command)
        {
            var activity = await _remarkService.AddActivityAsync(command);

            var dto = new ActivityDto
            {
                Id = activity.Id,
                Name = activity.Type?.Name
            };

            return Ok(dto);
        }

        /// <summary>
        /// Get comments
        /// </summary>
        [HttpGet("{id:guid}/comments")]
        public async Task<IActionResult> GetComments(Guid id)
        {
            var comments = await _remarkService.GetCommentsAsync(id);

            var result = comments
                .Select(x => new CommentDto
                {
                    Id = x.Id,
                    AuthorId = x.AuthorId,
                    Status = x.Status.ToString(),
                    Text = x.Text
                })
                .ToList();

            return Ok(result);
        }

        /// <summary>
        /// Add comments
        /// </summary>
        [HttpGet("{id:guid}/comments")]
        public async Task<IActionResult> AddComment(Guid id, [FromBody] AddComment command)
        {
            var comment = await _remarkService.AddCommentAsync(command);

            var dto = new CommentDto
            {
                Id = comment.Id,
                AuthorId = comment.AuthorId,
                Status = comment.Status.ToString(),
                Text = comment.Text
            };

            return Ok(dto);
        }

        /// <summary>
        /// Remove comment
        /// </summary>
        [HttpDelete("{id:guid}/comments/{commentId:guid}")]
        public async Task<IActionResult> RemoveComment(Guid id, Guid commentId)
        {
            var command = new RemoveComment
            {
                RemarkId = id,
                CommentId = commentId
            };
            var comment = await _remarkService.RemoveCommentAsync(command);

            var dto = new CommentDto
            {
                Id = comment.Id,
                AuthorId = comment.AuthorId,
                Status = comment.Status.ToString(),
                Text = comment.Text
            };

            return Ok(dto);
        }

        /// <summary>
        /// Get images
        /// </summary>
        [HttpGet("{id:guid}/images")]
        public async Task<IActionResult> GetImages(Guid id)
        {
            var images = await _remarkService.GetImagessAsync(id);

            var result = images
                .Select(x => new ImageDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Url = x.Url,
                })
                .ToList();

            return Ok(result);
        }

        /// <summary>
        /// Add image
        /// </summary>
        [HttpPost("{id:guid}/images")]
        public async Task<IActionResult> AddImage(Guid id, [FromBody] AddImage command)
        {
            var images = await _remarkService.AddImageAsync(command);

            var dto = new ImageDto
            {
                Id = images.Id,
                Name = images.Name,
                Url = images.Url,
            };

            return Ok(dto);
        }

        /// <summary>
        /// Remove image
        /// </summary>
        [HttpDelete("{id:guid}/images/{imageId:guid}")]
        public async Task<IActionResult> RemoveImage(Guid id, Guid imageId)
        {
            var command = new RemoveImage
            {
                ImageId = imageId,
                RemarkId = id
            };
            var images = await _remarkService.RemoveImageAsync(command);

            var dto = new ImageDto
            {
                Id = images.Id,
                Name = images.Name,
                Url = images.Url,
            };

            return Ok(dto);
        }
    }
}
