using System;
using System.Threading.Tasks;
using GP.Microservices.Common.Messages.Users.Commands;
using GP.Microservices.Users.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace GP.Microservices.Users.Controllers
{
    [Route("api/users")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Browse()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> Get(string username)
        {
            var result = await _userService.GetAsync(username);

            return Ok(result);
        }

        [HttpPost("{username}/authorize")]
        public async Task<IActionResult> Authorize(string username, [FromBody] AuthorizeUser command)
        {
            var result = await _userService.AuthorizeAsync(command);

            return Ok(result);
        }
    }
}