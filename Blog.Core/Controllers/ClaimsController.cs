using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Core.Controllers
{
    [Authorize("Permission")]
    [Route("api/Claims")]
    [ApiController]
    public class ClaimsController : Controller
    {
        // *****这是一个测试的控制器，主要为了测试基于Claim的验证机制*****
        // *****[Authorize("Permission")]*****




        // GET: api/Claims
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Claims/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Claims
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Claims/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
