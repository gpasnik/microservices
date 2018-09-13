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

        /// <summary>
        /// Browse users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Browse()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get user account
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet("{username}")]
        public async Task<IActionResult> Get(string username)
        {
            var result = await _userService.GetAsync(username);

            return Ok(result);
        }

        /// <summary>
        /// Create user
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("{username}")]
        public async Task<IActionResult> Create([FromBody] RegisterUser command)
        {
            var result = await _userService.RegisterAsync(command.Username, command.Password, command.Email,
                command.Name, command.LastName);

            return Ok(result);
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpDelete("{username}")]
        public async Task<IActionResult> Delete(string username)
        {
            var result = await _userService.DeleteAsync(username);

            return Ok(result);
        }

        /// <summary>
        /// Authorize user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("{username}/authorize")]
        public async Task<IActionResult> Authorize(string username, [FromBody] AuthorizeUser command)
        {
            var result = await _userService.AuthorizeAsync(command);

            return Ok(result);
        }

        /// <summary>
        /// Block user
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPut("{username}/block")]
        public async Task<IActionResult> Block(string username)
        {
            var result = await _userService.BlockAsync(username);

            return Ok(result);
        }

        /// <summary>
        /// Block user
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPut("{username}/unblock")]
        public async Task<IActionResult> Unblock(string username)
        {
            var result = await _userService.UnblockAsync(username);

            return Ok(result);
        }
    }
}