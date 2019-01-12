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
        public async Task<MessageModel<TopicDetail>> Get(int id)
        {
            var data = new MessageModel<TopicDetail>();
            data.Response = await _topicDetailServices.QueryByID(id);
            if (data.Response != null)
            {
                data.Success = true;
                data.Msg = "";
            }

            return data;
        }

        // POST: api/TopicDetail
        [HttpPost]
        public async Task<MessageModel<string>> Post([FromBody] TopicDetail topicDetail)
        {
            var data = new MessageModel<string>();

            if (topicDetail != null && topicDetail.Id > 0)
            {
                data.Success = await _topicDetailServices.Update(topicDetail);
                if (data.Success)
                {
                    data.Msg = "更新成功";
                    data.Response = topicDetail?.Id.ObjToString();
                }
            }
            else
            {
                topicDetail.tdCreatetime = DateTime.Now;
                topicDetail.tdRead = 0;
                topicDetail.tdCommend = 0;
                topicDetail.tdGood = 0;
                topicDetail.tdTop = 0;

                var id = (await _topicDetailServices.Add(topicDetail));
                data.Success = id > 0;
                if (data.Success)
                {
                    data.Response = id.ObjToString();
                    data.Msg = "添加成功";
                }
            }

            return data;
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
