using System;
using System.Threading.Tasks;
using GP.Microservices.Common.Dto;
using GP.Microservices.Common.Messages.Users.Commands;
using Microsoft.AspNetCore.Mvc;

namespace GP.Microservices.Users.Controllers
{
    [Route("api/users")]
    public class UsersController : Controller
    {
        public UsersController()
        {
            
        }

        [HttpGet]
        public async Task<IActionResult> Browse()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> Get(string username)
        {
            throw new NotImplementedException();
        }

        [HttpPost("{username}/authorize")]
        public async Task<IActionResult> Authorize(string username, [FromBody] AuthorizeUser command)
        {
            if (command.Username == "user" && command.Password == "password")
            {
                var user = await Task.FromResult(new UserDto
                {
                    Id = Guid.NewGuid(),
                    Username = command.Username
                });

                return Ok(user);
            }

            return Unauthorized();
        }
    }
}