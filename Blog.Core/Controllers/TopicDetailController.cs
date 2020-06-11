using System;
using System.Linq;
using System.Threading.Tasks;
using Blog.Core.Common.Helper;
using Blog.Core.IServices;
using Blog.Core.Model;
using Blog.Core.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Core.Controllers
{
    /// <summary>
    /// Tibug 管理
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Permissions.Name)]
    public class TopicDetailController : ControllerBase
    {
        readonly ITopicServices _topicServices;
        readonly ITopicDetailServices _topicDetailServices;

        /// <summary>
        /// 构造函数
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
        /// 【无权限】
        /// </summary>
        /// <param name="page">页数</param>
        /// <param name="tname">专题类型</param>
        /// <param name="key">关键字</param>
        /// <param name="intPageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<PageModel<TopicDetail>>> Get(int page = 1, string tname = "", string key = "", int intPageSize = 12)
        {
            int tid = 0;

            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            {
                key = "";
            }
            if (string.IsNullOrEmpty(tname) || string.IsNullOrWhiteSpace(tname))
            {
                tname = "";
            }
            tname = UnicodeHelper.UnicodeToString(tname);

            if (!string.IsNullOrEmpty(tname))
            {
                tid = ((await _topicServices.Query(ts => ts.tName == tname)).FirstOrDefault()?.Id).ObjToInt();
            }


            var data = await _topicDetailServices.QueryPage(a => !a.tdIsDelete && a.tdSectendDetail == "tbug" && ((tid == 0 && true) || (tid > 0 && a.TopicId == tid)) && ((a.tdName != null && a.tdName.Contains(key)) || (a.tdDetail != null && a.tdDetail.Contains(key))), page, intPageSize, " Id desc ");



            return new MessageModel<PageModel<TopicDetail>>()
            {
                msg = "获取成功",
                success = data.dataCount >= 0,
                response = data
            };

        }

        /// <summary>
        /// 获取详情【无权限】
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/TopicDetail/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<MessageModel<TopicDetail>> Get(int id)
        {
            var data = new MessageModel<TopicDetail>();
            var response = id > 0 ? await _topicDetailServices.QueryById(id) : new TopicDetail();
            data.response = (response?.tdIsDelete).ObjToBool() ? new TopicDetail() : response;
            if (data.response != null)
            {
                data.success = true;
                data.msg = "";
            }

            return data;
        }

        /// <summary>
        /// 添加一个 BUG 【无权限】
        /// </summary>
        /// <param name="topicDetail"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 更新 bug
        /// </summary>
        /// <param name="topicDetail"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 删除 bug
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/ApiWithActions/5
        [HttpDelete]
        public async Task<MessageModel<string>> Delete(int id)
        {
            var data = new MessageModel<string>();
            if (id > 0)
            {
                var topicDetail = await _topicDetailServices.QueryById(id);
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
