using System.Threading.Tasks;
using GP.Microservices.Api.Models;
using GP.Microservices.Common.Authentication;
using GP.Microservices.Common.Messages.Users.Commands;
using GP.Microservices.Common.ServiceClients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GP.Microservices.Api.Controllers
{
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IJwtTokenService _jwtHandler;
        private readonly IUserServiceClient _userService;

        public AccountController(
            IJwtTokenService jwtHandler,
            IUserServiceClient userService)
        {
            _jwtHandler = jwtHandler;
            _userService = userService;
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Get()
        {
            //TODO: get username from token
            var username = "get username from token";

            var result = await _userService.GetUserAsync(username);

            return Ok(result);
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] SignIn request)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var command = new AuthorizeUser
            {
                Username = request.Username,
                Password = request.Password
            };

            var result = await _userService.AuthorizeUserAsync(command);

            if (result.Failure)
            {
                return StatusCode(403, new ApiError {ErrorCode = result.Error.Code, Message = result.Error.Code});
            }

            var token = _jwtHandler.Create(request.Username);

            return Ok(token);
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] SignUp request)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var command = new RegisterUser
            {
                Email = request.Email,
                Username = request.Username,
                Password = request.Password,
                Name = request.Name ?? string.Empty,
                LastName = request.LastName ?? string.Empty
            };

            var result = await _userService
                .RegisterUserAsync(command)
                .OrFailAsync();

            return Ok(result.Username);
        }

        [HttpDelete("me")]
        public async Task<IActionResult> Delete()
        {
            //TODO: get username from token
            var username = "get username from token";

            var command = new DeleteUser
            {
                Username = username
            };

            var result = await _userService
                .DeleteUserAsync(command)
                .OrFailAsync();

            return Ok(result);
        }
    }
}