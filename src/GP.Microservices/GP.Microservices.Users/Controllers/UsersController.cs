using System;
using System.Linq;
using System.Threading.Tasks;
using GP.Microservices.Common.Dto;
using GP.Microservices.Common.Messages.Remarks.Queries;
using GP.Microservices.Common.Messages.Users.Commands;
using GP.Microservices.Users.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace GP.Microservices.Users.Controllers
{
    /// <summary>
    /// User resource endpoints
    /// </summary>
    [Route("api/users")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        /// <inheritdoc />
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
            var query = new BrowseUsers();
            var users = await _userService.BrowseAsync(query);

            var result = users
                .Select(x => new UserDto
                {
                    Id = x.Id,
                    Username = x.Username,
                    Email = x.Email,
                    Name = x.Name,
                    Lastname = x.Lastname,
                    Status = x.Status.ToString()
                })
                .ToList();

            return Ok(result);
        }

        /// <summary>
        /// Get user account
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet("{username}")]
        public async Task<IActionResult> Get(string username)
        {
            var user = await _userService.GetAsync(username);
            var dto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Name = user.Name,
                Lastname = user.Lastname,
                Status = user.Status.ToString()
            };

            return Ok(dto);
        }

        /// <summary>
        /// Create user
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("{username}")]
        public async Task<IActionResult> Create([FromBody] RegisterUser command)
        {
            var user = await _userService.RegisterAsync(command.Username, command.Password, command.Email,
                command.Name, command.LastName);
            var dto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Name = user.Name,
                Lastname = user.Lastname,
                Status = user.Status.ToString()
            };

            return Ok(dto);
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpDelete("{username}")]
        public async Task<IActionResult> Delete(string username)
        {
            var user = await _userService.DeleteAsync(username);
            var dto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Name = user.Name,
                Lastname = user.Lastname,
                Status = user.Status.ToString()
            };

            return Ok(dto);
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
            var user = await _userService.AuthorizeAsync(command);
            var dto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Name = user.Name,
                Lastname = user.Lastname,
                Status = user.Status.ToString()
            };

            return Ok(dto);
        }

        /// <summary>
        /// Block user
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPut("{username}/block")]
        public async Task<IActionResult> Block(string username)
        {
            var user = await _userService.BlockAsync(username);
            var dto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Name = user.Name,
                Lastname = user.Lastname,
                Status = user.Status.ToString()
            };

            return Ok(dto);
        }

        /// <summary>
        /// Block user
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPut("{username}/unblock")]
        public async Task<IActionResult> Unblock(string username)
        {
            var user = await _userService.UnblockAsync(username);
            var dto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Name = user.Name,
                Lastname = user.Lastname,
                Status = user.Status.ToString()
            };

            return Ok(dto);
        }
    }
}