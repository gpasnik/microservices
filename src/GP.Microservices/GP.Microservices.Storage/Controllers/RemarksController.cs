using System;
using System.Threading.Tasks;
using GP.Microservices.Common.Messages.Remarks.Queries;
using GP.Microservices.Storage.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GP.Microservices.Storage.Controllers
{
    [Route("api/remarks")]
    public class RemarksController : Controller
    {
        private readonly IRemarkRepository _remarkRepository;
        private readonly IRemarkCategoryRepository _categoryRepository;
        private readonly IActivityTypeRepository _activityTypeRepository;

        /// <inheritdoc />
        public RemarksController(
            IRemarkRepository remarkRepository,
            IRemarkCategoryRepository categoryRepository,
            IActivityTypeRepository activityTypeRepository)
        {
            _remarkRepository = remarkRepository;
            _categoryRepository = categoryRepository;
            _activityTypeRepository = activityTypeRepository;
        }

        /// <summary>
        /// Browse remarks
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var query = new BrowseRemarks();
            var result = await _remarkRepository.BrowseAsync(query);

            return Ok(result);
        }

        /// <summary>
        /// Get remark
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var remark = await _remarkRepository.GetAsync(id);

            return Ok(remark);
        }

        /// <summary>
        /// Get remark categories
        /// </summary>
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryRepository.GetAsync();

            return Ok(categories);
        }

        /// <summary>
        /// Get remark categories
        /// </summary>
        [HttpGet("activities")]
        public async Task<IActionResult> GetActivities()
        {
            var categories = await _activityTypeRepository.GetAsync();

            return Ok(categories);
        }
    }
}
