using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Core.Controllers
{
    /// <summary>
    /// 这是一个测试的控制器，主要为了测试基于Claim的验证机制
    /// </summary>
    //[Authorize(PermissionNames.Permission)]
    [Route("api/Claims/[action]")]
    [ApiController]
    public class ClaimsController : Controller
    {
        // *****这是一个测试的控制器，主要为了测试基于Claim的验证机制*****
        // *****[Authorize(PermissionNames.Permission)]*****




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

        /// <summary>
        /// 测试批量删除，如果是axios，记得要把数组格式化成 stringQuery
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        public string BatchDelete(string ids)
        {
            return ids;
        }
    }
}
