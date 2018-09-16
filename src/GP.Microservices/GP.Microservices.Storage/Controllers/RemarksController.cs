using System;
using System.Threading.Tasks;
using GP.Microservices.Common.Messages.Remarks.Queries;
using GP.Microservices.Storage.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GP.Microservices.Storage.Controllers
{
    [Route("api/remarks")]
    public class RemarksController : Controller
    {
        private readonly IRemarkRepository _remarkRepository;

        /// <inheritdoc />
        public RemarksController(IRemarkRepository remarkRepository)
        {
            _remarkRepository = remarkRepository;
        }

        /// <summary>
        /// Browse remarks
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var query = new BrowseRemarks();
            var result = await _remarkRepository.BrowseAsync(query);

            return Ok(result);
        }

        /// <summary>
        /// Get remark
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var remark = await _remarkRepository.GetAsync(id);

            return Ok(remark);
        }
    }
}
