using System.Linq;
using System.Threading.Tasks;
using GP.Microservices.Common.Dto;
using GP.Microservices.Remarks.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace GP.Microservices.Remarks.Controllers
{
    /// <summary>
    /// Activities resource endpoints
    /// </summary>
    [Route("api/activities")]
    public class ActivitiesController : Controller
    {
        private readonly IActivityService _activityService;

        /// <inheritdoc />
        public ActivitiesController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        /// <summary>
        /// Get activity types
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var activities = await _activityService.GetAsync();

            var result = activities
                .Select(x => new ActivityTypeDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToList();

            return Ok(result);
        }
    }
}