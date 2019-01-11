using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Core.IServices;
using Blog.Core.Model.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicController : ControllerBase
    {
        ITopicServices _topicServices;

        public TopicController(ITopicServices topicServices)
        {
            _topicServices = topicServices;
        }

        // GET: api/Topic
        [HttpGet]
        public async Task<object> Get()
        {
            List<Topic> topics = new List<Topic>();

            try
            {

                topics = await _topicServices.Query(a => !a.tIsDelete && a.tSectendDetail == "tbug");
            }
            catch (Exception) { }

            return Ok(new
            {
                success = topics.Any(),
                data = topics
            });
        }

        // GET: api/Topic/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Topic
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Topic/5
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
