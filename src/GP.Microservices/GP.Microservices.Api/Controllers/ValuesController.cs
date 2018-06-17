using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GP.Microservices.Common;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace GP.Microservices.Api.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IBus _bus;

        public ValuesController(IBus bus)
        {
            _bus = bus;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2", "api" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]string value)
        {
            try
            {
                var message = new SampleMessage
                {
                    Id = Guid.NewGuid(),
                    Message = value
                };
                await _bus.Publish(message);

                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
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
