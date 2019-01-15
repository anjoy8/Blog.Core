using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Core.IServices;
using Blog.Core.Model;
using Blog.Core.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Permission")]
    public class TopicDetailController : ControllerBase
    {
        ITopicServices _topicServices;
        ITopicDetailServices _topicDetailServices;

        public TopicDetailController(ITopicServices topicServices, ITopicDetailServices topicDetailServices)
        {
            _topicServices = topicServices;
            _topicDetailServices = topicDetailServices;
        }

        /// <summary>
        /// 获取Bug数据列表（带分页）
        /// </summary>
        /// <param name="page">页数</param>
        /// <param name="tname">专题类型</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<PageModel<TopicDetail>>> Get(int page = 1, string tname = "")
        {
            var data = new MessageModel<PageModel<TopicDetail>>();
            int intTotalCount = 6;
            int TotalCount = 0;
            int PageCount = 1;
            List<TopicDetail> topicDetails = new List<TopicDetail>();

            topicDetails = await _topicDetailServices.Query(a => !a.tdIsDelete && a.tdSectendDetail == "tbug");
            if (!string.IsNullOrEmpty(tname))
            {
                var tid = (await _topicServices.Query(ts => ts.tName == tname)).FirstOrDefault()?.Id.ObjToInt();
                topicDetails = topicDetails.Where(t => t.TopicId == tid).ToList();
            }
            //数据总数
            TotalCount = topicDetails.Count;

            //总页数
            PageCount = (Math.Ceiling(topicDetails.Count.ObjToDecimal() / intTotalCount.ObjToDecimal())).ObjToInt();

            topicDetails = topicDetails.OrderByDescending(d => d.Id).Skip((page - 1) * intTotalCount).Take(intTotalCount).ToList();

            return new MessageModel<PageModel<TopicDetail>>()
            {
                Msg = "获取成功",
                Success = topicDetails.Count >= 0,
                Response = new PageModel<TopicDetail>()
                {
                    page = page,
                    pageCount = PageCount,
                    dataCount = TotalCount,
                    data = topicDetails,
                }
            };

        }

        // GET: api/TopicDetail/5
        [HttpGet("{id}")]
        [AllowAnonymous]
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
        [AllowAnonymous]
        public async Task<MessageModel<string>> Post([FromBody] TopicDetail topicDetail)
        {
            var data = new MessageModel<string>();

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

            return data;
        }

        // PUT: api/TopicDetail/5
        [HttpPut]
        [Route("update")]
        [AllowAnonymous]
        public async Task<MessageModel<string>> Put(int id, [FromBody] TopicDetail topicDetail)
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

            return data;
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
