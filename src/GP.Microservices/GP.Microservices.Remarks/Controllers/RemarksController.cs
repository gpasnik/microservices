using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GP.Microservices.Common.Dto;
using GP.Microservices.Common.Messages.Remarks.Commands;
using GP.Microservices.Remarks.Domain.Services;
using Microsoft.AspNetCore.Mvc;

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

        public RemarksController(IRemarkService remarkService,
            ICategoryService categorySevice)
        {
            _remarkService = remarkService;
            _categorySevice = categorySevice;
        }

        /// <summary>
        /// Browse remarks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Browse()
        {
            return NoContent();
        }

        /// <summary>
        /// Create remark
        /// </summary>
        /// <param name="command"></param>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateRemark command)
        {
            var result = await _remarkService.CreateAsync(command);

            var dto = new RemarkDto
            {
                Id = result.Id,
                Name = result.Name,
                Author = result.AuthorId
            };

            return Ok(dto);
        }

        /// <summary>
        /// Get remark details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var remark = await _remarkService.GetAsync(id);
            var activities = await _remarkService.GetActivitiesAsync(id);
            var comments = await _remarkService.GetCommentsAsync(id);
            var images = await _remarkService.GetImagessAsync(id);
            var category = await _categorySevice.GetAsync(remark.CategoryId);

            var dto = new RemarkDto
            {
                Id = remark.Id,
                Name = remark.Name,
                Description = remark.Description,
                //Latitude
                //Longitude
                Author = remark.AuthorId,
                CategoryId = remark.CategoryId,
                CategoryName = category.Name,
                Status = remark.Status,
                Activities = activities?
                    .Select(x => new ActivityDto
                    {
                        
                    })
                    .ToList(),
                Comments = comments
                    .Select(x => new CommentDto())
                    .ToList(),
                Images = images
                    .Select(x => new ImageDto())
                    .ToList()
            };
            return Ok(dto);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

    /// <summary>
    /// Activities resource endpoints
    /// </summary>
    [Route("api/activities")]
    public class ActivitiesController : Controller
    {

    }
}
