using Microsoft.AspNetCore.Mvc;

namespace GP.Microservices.Api.Controllers
{
    public class UsersController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return NoContent();
        }
    }
}