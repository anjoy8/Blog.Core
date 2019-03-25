using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Core.IServices;
using Blog.Core.Model;
using Blog.Core.Model.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicController : ControllerBase
    {
        readonly ITopicServices _topicServices;

        public TopicController(ITopicServices topicServices)
        {
            _topicServices = topicServices;
        }

        // GET: api/Topic
        [HttpGet]
        public async Task<MessageModel<List<Topic>>> Get()
        {
            var data = new MessageModel<List<Topic>> {response = await _topicServices.GetTopics()};
            if (data.response != null)
            {
                data.success = true;
                data.msg = "";
            }
            return data;
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
