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
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize("Permission")]
    public class TopicDetailController : ControllerBase
    {
        ITopicServices _topicServices;
        ITopicDetailServices _topicDetailServices;

        /// <summary>
        /// TopicDetailController
        /// </summary>
        /// <param name="topicServices"></param>
        /// <param name="topicDetailServices"></param>
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
        public async Task<MessageModel<PageModel<TopicDetail>>> Get(int page = 1, string tname = "", string key = "")
        {
            var data = new MessageModel<PageModel<TopicDetail>>();
            int intTotalCount = 6;
            int TotalCount = 0;
            int PageCount = 1;
            List<TopicDetail> topicDetails = new List<TopicDetail>();

            //总数据，使用AOP切面缓存
            //topicDetails = await _topicDetailServices.GetTopicDetails();
            topicDetails = await _topicDetailServices.Query(a => !a.tdIsDelete && a.tdSectendDetail == "tbug");

            if (!string.IsNullOrEmpty(key))
            {
                topicDetails = topicDetails.Where(t => (t.tdName != null && t.tdName.Contains(key)) || (t.tdDetail != null && t.tdDetail.Contains(key))).ToList();
            }

            if (!string.IsNullOrEmpty(tname))
            {
                var tid = (await _topicServices.Query(ts => ts.tName == tname)).FirstOrDefault()?.Id.ObjToInt();
                topicDetails = topicDetails.Where(t => t.TopicId == tid).ToList();
            }

            //筛选后的数据总数
            TotalCount = topicDetails.Count;
            //筛选后的总页数
            PageCount = (Math.Ceiling(TotalCount.ObjToDecimal() / intTotalCount.ObjToDecimal())).ObjToInt();

            topicDetails = topicDetails.OrderByDescending(d => d.Id).Skip((page - 1) * intTotalCount).Take(intTotalCount).ToList();

            return new MessageModel<PageModel<TopicDetail>>()
            {
                msg = "获取成功",
                success = TotalCount >= 0,
                response = new PageModel<TopicDetail>()
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
            var response = await _topicDetailServices.QueryByID(id);
            data.response = response.tdIsDelete ? null : response;
            if (data.response != null)
            {
                data.success = true;
                data.msg = "";
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
            data.success = id > 0;
            if (data.success)
            {
                data.response = id.ObjToString();
                data.msg = "添加成功";
            }

            return data;
        }

        // PUT: api/TopicDetail/5
        [HttpPut]
        public async Task<MessageModel<string>> Update([FromBody] TopicDetail topicDetail)
        {
            var data = new MessageModel<string>();
            if (topicDetail != null && topicDetail.Id > 0)
            {
                data.success = await _topicDetailServices.Update(topicDetail);
                if (data.success)
                {
                    data.msg = "更新成功";
                    data.response = topicDetail?.Id.ObjToString();
                }
            }

            return data;
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete]
        public async Task<MessageModel<string>> Delete(int id)
        {
            var data = new MessageModel<string>();
            if (id > 0)
            {
                var topicDetail = await _topicDetailServices.QueryByID(id);
                topicDetail.tdIsDelete = true;
                data.success = await _topicDetailServices.Update(topicDetail);
                if (data.success)
                {
                    data.msg = "删除成功";
                    data.response = topicDetail?.Id.ObjToString();
                }
            }

            return data;
        }
    }
}
