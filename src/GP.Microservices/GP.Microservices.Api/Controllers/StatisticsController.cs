using Microsoft.AspNetCore.Mvc;

namespace GP.Microservices.Api.Controllers
{
    public class StatisticsController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return NoContent();
        }
    }
}