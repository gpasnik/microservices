using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GP.Microservices.Common.Messages.Remarks.Commands;
using Microsoft.AspNetCore.Mvc;

namespace GP.Microservices.Remarks.Controllers
{
    /// <summary>
    /// Remark resource endpoints
    /// </summary>
    [Route("api/remarks")]
    public class RemarksController : Controller
    {
        /// <summary>
        /// Browse remarks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Browse()
        {
            return NoContent();
        }

        /// <summary>
        /// Create remark
        /// </summary>
        /// <param name="command"></param>
        [HttpPost]
        public IActionResult Post([FromBody] CreateRemark command)
        {
            return NoContent();
        }

        /// <summary>
        /// Get remark details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public IActionResult Get(Guid id)
        {
            return NoContent();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
