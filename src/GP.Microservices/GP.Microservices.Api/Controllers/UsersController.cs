using System.Threading.Tasks;
using GP.Microservices.Common.Messages.Remarks.Queries;
using GP.Microservices.Common.Messages.Users.Commands;
using GP.Microservices.Common.ServiceClients;
using Microsoft.AspNetCore.Mvc;

namespace GP.Microservices.Api.Controllers
{
    [Route("api/users")]
    public class UsersController : Controller
    {
        private readonly IUserServiceClient _userService;
        private readonly IStorageServiceClient _storageService;

        public UsersController(
            IUserServiceClient userService,
            IStorageServiceClient storageService)
        {
            _userService = userService;
            _storageService = storageService;
        }

        [HttpGet]
        public async Task<IActionResult> Browse()
        {
            var query = new BrowseUsers();
            var result = await _storageService.BrowseUsersAsync(query);

            return Ok(result);
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> Get(string username)
        {
            var result = await _storageService.GetUserAsync(username);

            return Ok(result);
        }

        [HttpPut("{username}")]
        public async Task<IActionResult> Block(string username)
        {
            var command = new BlockUser
            {
                Username = username
            };

            var result = await _userService.BlockUserAsync(command);

            return Ok(result);
        }

        [HttpPut("{username}")]
        public async Task<IActionResult> Unblock(string username)
        {
            var command = new UnblockUser
            {
                Username = username
            };

            var result = await _userService.UnblockUserAsync(command);

            return Ok(result);
        }
    }
}