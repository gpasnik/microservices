using Microsoft.AspNetCore.Mvc;

namespace GP.Microservices.Api.Controllers
{
    public class RemarksController : ControllerBase
    {
        // GET
        public IActionResult Index()
        {
            return NoContent();
        }
    }
}