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
    public class TopicDetailController : ControllerBase
    {
        ITopicServices _topicServices;
        ITopicDetailServices _topicDetailServices;

        public TopicDetailController(ITopicServices topicServices, ITopicDetailServices topicDetailServices)
        {
            _topicServices = topicServices;
            _topicDetailServices = topicDetailServices;
        }

        // GET: api/TopicDetail
        [HttpGet]
        public async Task<object> Get(int page = 1, string tname = "")
        {
            int intTotalCount = 6;
            int TotalCount = 1;
            List<TopicDetail> topicDetails = new List<TopicDetail>();

            topicDetails = await _topicDetailServices.Query(a => !a.tdIsDelete && a.tdSectendDetail == "tbug");
            if (!string.IsNullOrEmpty(tname))
            {
                var tid = (await _topicServices.Query(ts => ts.tName == tname)).FirstOrDefault()?.Id.ObjToInt();
                topicDetails = topicDetails.Where(t => t.TopicId == tid).ToList();
            }

            topicDetails = topicDetails.OrderByDescending(d => d.Id).Skip((page - 1) * intTotalCount).Take(intTotalCount).ToList();

            return Ok(new
            {
                success = true,
                page = page,
                pageCount = TotalCount,
                Article = topicDetails
            });
        }

        // GET: api/TopicDetail/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/TopicDetail
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/TopicDetail/5
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
