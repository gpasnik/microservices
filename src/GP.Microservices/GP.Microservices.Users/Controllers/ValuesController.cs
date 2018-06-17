using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GP.Microservices.Users.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GP.Microservices.Users.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly UsersContext _usersContext;

        public ValuesController(UsersContext usersContext)
        {
            _usersContext = usersContext;
        }

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<Message>> Get()
        {
            try
            {
                var messages = await _usersContext
                    .Messages
                    .ToListAsync();

                return messages;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
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
