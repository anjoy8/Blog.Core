using System.Threading.Tasks;
using Blog.Core.IServices;
using Blog.Core.Model;
using Blog.Core.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Core.Controllers
{
    /// <summary>
	/// WeChatConfigController
	/// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Permissions.Name)]
    public partial class WeChatConfigController : Controller
    {
        readonly IWeChatConfigServices _WeChatConfigServices;
        /// <summary>
        /// 构造函数
        /// </summary> 
        /// <param name="iWeChatConfigServices"></param> 
        public WeChatConfigController(IWeChatConfigServices iWeChatConfigServices)
        {
            _WeChatConfigServices = iWeChatConfigServices;
        } 
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="pagination">分页条件</param> 
        /// <returns></returns>
        [HttpGet]  
        public async Task<MessageModel<PageModel<WeChatConfig>>> Get([FromQuery] PaginationModel pagination)
        { 
            var data = await _WeChatConfigServices.QueryPage(pagination);
            return new MessageModel<PageModel<WeChatConfig>> { success = true, response = data};
        }  
        /// <summary>
        /// 获取(id)
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<MessageModel<WeChatConfig>> Get(string id)
        {
            var data = await _WeChatConfigServices.QueryById(id);
            return new MessageModel<WeChatConfig> { success = true, response = data };
        } 
        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<string>> Post([FromBody] WeChatConfig obj)
        {
            await _WeChatConfigServices.Add(obj);
            return new MessageModel<string> { success = true};
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<MessageModel<string>> Put([FromBody] WeChatConfig obj)
        {
            await _WeChatConfigServices.Update(obj);
            return new MessageModel<string> { success = true};
        }
        /// <summary>
        /// 删除
        /// </summary> 
        /// <returns></returns> 
        [HttpDelete]
        public async Task<MessageModel<string>> Delete(string id)
        {
            await _WeChatConfigServices.DeleteById(id);
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
            await _WeChatConfigServices.DeleteByIds(i);
            return new MessageModel<string> { success = true };
        }

    }
}