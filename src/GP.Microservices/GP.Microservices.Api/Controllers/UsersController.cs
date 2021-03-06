﻿using System.Threading.Tasks;
using GP.Microservices.Common.Messages.Remarks.Queries;
using GP.Microservices.Common.Messages.Users.Commands;
using GP.Microservices.Common.ServiceClients;
using Microsoft.AspNetCore.Mvc;

namespace GP.Microservices.Api.Controllers
{
    /// <summary>
    /// User endpoint
    /// </summary>
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserServiceClient _userService;
        private readonly IStorageServiceClient _storageService;

        /// <summary>
        /// Constructor
        /// </summary>
        public UsersController(
            IUserServiceClient userService,
            IStorageServiceClient storageService)
        {
            _userService = userService;
            _storageService = storageService;
        }

        /// <summary>
        /// Browse users
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Browse()
        {
            var query = new BrowseUsers();
            var result = await _storageService
                .BrowseUsersAsync(query)
                .OrFailAsync();

            return Ok(result);
        }

        /// <summary>
        /// Get user
        /// </summary>
        /// <param name="username">Username of the user to get</param>
        /// <returns></returns>
        [HttpGet("{username}")]
        public async Task<IActionResult> Get(string username)
        {
            var result = await _storageService
                .GetUserAsync(username)
                .OrFailAsync();

            return Ok(result);
        }

        /// <summary>
        /// Block user
        /// </summary>
        /// <param name="username">Username of the user to block</param>
        /// <returns></returns>
        [HttpPut("{username}/block")]
        public async Task<IActionResult> Block(string username)
        {
            var command = new BlockUser
            {
                Username = username
            };

            var result = await _userService
                .BlockUserAsync(command)
                .OrFailAsync();

            return Ok(result);
        }

        /// <summary>
        /// Unblock user
        /// </summary>
        /// <param name="username">Username of the user to unblock</param>
        /// <returns></returns>
        [HttpPut("{username}/unblock")]
        public async Task<IActionResult> Unblock(string username)
        {
            var command = new UnblockUser
            {
                Username = username
            };

            var result = await _userService
                .UnblockUserAsync(command)
                .OrFailAsync();

            return Ok(result);
        }
    }
}