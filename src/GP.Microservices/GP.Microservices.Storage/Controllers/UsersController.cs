using System;
using System.Threading.Tasks;
using GP.Microservices.Common.Messages.Remarks.Queries;
using GP.Microservices.Storage.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GP.Microservices.Storage.Controllers
{
    [Route("api/users")]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;

        /// <inheritdoc />
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Browse users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var query = new BrowseUsers();
            var result = await _userRepository.BrowseAsync(query);

            return Ok(result);
        }

        /// <summary>
        /// Get user
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var remark = await _userRepository.GetAsync(id);

            return Ok(remark);
        }
    }
}