using Microsoft.AspNetCore.Mvc;

namespace GP.Microservices.Api.Controllers
{
    [Route("api/[controller]")]
    public class StatisticsController : Controller
    {
        /// <summary>
        /// Browse statistics
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Browse()
        {
            return NoContent();
        }
    }
}