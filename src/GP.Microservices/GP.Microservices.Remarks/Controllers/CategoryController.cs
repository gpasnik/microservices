using System.Linq;
using System.Threading.Tasks;
using GP.Microservices.Common.Dto;
using GP.Microservices.Remarks.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace GP.Microservices.Remarks.Controllers
{
    /// <summary>
    /// Category resource endpoints
    /// </summary>
    [Route("api/categories")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        /// <inheritdoc />
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Get categories
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var categories = await _categoryService.GetAsync();

            var result = categories
                .Select(x => new RemarkCategoryDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToList();

            return Ok(result);
        }
    }
}