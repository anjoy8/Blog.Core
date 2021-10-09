using System.Collections.Generic;
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
	/// WeChatPushLogController
	/// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Permissions.Name)]
    public partial class WeChatPushLogController : Controller
    {
        readonly IWeChatPushLogServices _WeChatPushLogServices;
        /// <summary>
        /// 构造函数
        /// </summary> 
        /// <param name="iWeChatPushLogServices"></param> 
        public WeChatPushLogController(IWeChatPushLogServices iWeChatPushLogServices)
        {
            _WeChatPushLogServices = iWeChatPushLogServices;
        } 
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="pagination">分页条件</param> 
        /// <returns></returns>
        [HttpGet]  
        public async Task<MessageModel<PageModel<WeChatPushLog>>> Get([FromQuery] PaginationModel pagination)
        { 
            var data = await _WeChatPushLogServices.QueryPage(pagination);
            return new MessageModel<PageModel<WeChatPushLog>> { success = true, response = data};
        }  
        /// <summary>
        /// 获取(id)
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<MessageModel<WeChatPushLog>> Get(string id)
        {
            var data = await _WeChatPushLogServices.QueryById(id);
            return new MessageModel<WeChatPushLog> { success = true, response = data };
        } 
        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<string>> Post([FromBody] WeChatPushLog obj)
        {
            await _WeChatPushLogServices.Add(obj);
            return new MessageModel<string> { success = true};
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<MessageModel<string>> Put([FromBody] WeChatPushLog obj)
        {
            await _WeChatPushLogServices.Update(obj);
            return new MessageModel<string> { success = true};
        }
        /// <summary>
        /// 删除
        /// </summary> 
        /// <returns></returns> 
        [HttpDelete]
        public async Task<MessageModel<string>> Delete(string id)
        {
            await _WeChatPushLogServices.DeleteById(id);
            return new MessageModel<string> { success = true};
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public async Task<MessageModel<string>> BatchDelete(string ids)
        {
            var i = ids.Split(",");
            await _WeChatPushLogServices.DeleteByIds(i);
            return new MessageModel<string> { success = true };
        }

    }
}