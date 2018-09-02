using GP.Microservices.Api.Models;
using GP.Microservices.Common.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GP.Microservices.Api.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly IJwtTokenService _jwtHandler;

        public AccountController(IJwtTokenService jwtHandler)
        {
            _jwtHandler = jwtHandler;
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult Get()
        {
            return Content($"Hello {User.Identity.Name}");
        }

        [HttpPost("sign-in")]
        public IActionResult SignIn([FromBody]SignIn request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || request.Password != "secret")
            {
                return Unauthorized();
            }

            var result = _jwtHandler.Create(request.Username);

            return Ok(result);
        }
    }
}