using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Core.Controllers
{
    /// <summary>
    /// Values控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [Authorize(Roles = "Admin,Client")]
    [Authorize(Policy = "SystemOrAdmin")]
    public class ValuesController : ControllerBase
    {
        /// <summary>
        /// Get方法
        /// </summary>
        /// <returns></returns>
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }
        /// <summary>
        /// Get(int id)方法
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/values/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }
        /// <summary>
        /// post
        /// </summary>
        /// <param name="love">model实体类参数</param>
        [HttpPost]
        public void Post(Love love)
        {
        }
        /// <summary>
        /// Put方法
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
        /// <summary>
        /// Delete方法
        /// </summary>
        /// <param name="id"></param>
        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
