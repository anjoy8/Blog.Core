using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Blog.Core.Common.Helper;
using Blog.Core.IServices;
using Blog.Core.Model;
using Blog.Core.Model.Models;
using Blog.Core.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Blog.Core.Controllers
{
    /// <summary>
    /// 微信公众号管理 
    /// 作者:胡丁文
    /// 时间:2020-3-29 21:24:12
    /// </summary>   
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Permissions.Name)]
    public partial class WeChatController : Controller
    {
        readonly IWeChatConfigServices _weChatConfigServices;
        readonly ILogger<WeChatController> _logger; 
        /// <summary>
        /// 构造函数
        /// </summary>  
        /// <param name="weChatConfigServices"></param>
        /// <param name="logger"></param>   
        public WeChatController(IWeChatConfigServices weChatConfigServices, ILogger<WeChatController> logger)
        {
            _weChatConfigServices = weChatConfigServices;
            _logger = logger; 
        }
        /// <summary>
        /// 更新Token
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns> 
        [HttpGet]
        public async Task<MessageModel<WeChatApiDto>> GetToken(string id)
        {
            return await _weChatConfigServices.GetToken(id); 

        }
        /// <summary>
        /// 刷新Token
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns> 
        [HttpGet]
        public async Task<MessageModel<WeChatApiDto>> RefreshToken(string id)
        {
            return await _weChatConfigServices.RefreshToken(id);

        }
        /// <summary>
        /// 获取模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns> 
        [HttpGet] 
        public async Task<MessageModel<WeChatApiDto>> GetTemplate(string id)
        {
            return await _weChatConfigServices.GetTemplate(id);
        }
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns> 
        [HttpGet]
        public async Task<MessageModel<WeChatApiDto>> GetMenu(string id)
        {
            return await _weChatConfigServices.GetMenu(id);
        }

        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns> 
        [HttpPut]
        public async Task<MessageModel<WeChatApiDto>> UpdateMenu(WeChatApiDto menu)
        {
            return await _weChatConfigServices.UpdateMenu(menu);
        }
        /// <summary>
        /// 获取订阅用户(所有)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet] 
        public async Task<MessageModel<WeChatApiDto>> GetSubUsers(string id)
        {
            return await _weChatConfigServices.GetSubUsers(id);
        }
        /// <summary>
        /// 入口
        /// </summary>
        /// <param name="validDto"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [HttpGet]
        public async Task<string> Valid([FromQuery] WeChatValidDto validDto)
        {
            using (var reader = new StreamReader(Request.Body))
            {
                var body = await reader.ReadToEndAsync();
                return await _weChatConfigServices.Valid(validDto, body);
            }
        }
        /// <summary>
        /// 获取订阅用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<WeChatApiDto>> GetSubUser(string id,string openid)
        {
            return await _weChatConfigServices.GetSubUser(id,openid);
        }
        /// <summary>
        /// 获取一个绑定员工公众号二维码
        /// </summary>
        /// <param name="info">消息</param> 
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<WeChatResponseUserInfo>> GetQRBind([FromQuery]WeChatUserInfo info)
        {
            return await _weChatConfigServices.GetQRBind(info);
        }
        /// <summary>
        /// 推送卡片消息接口
        /// </summary>
        /// <param name="msg">卡片消息对象</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<MessageModel<WeChatResponseUserInfo>> PushCardMsg(WeChatCardMsgDataDto msg)
        {
            string pushUserIP = $"{Request.HttpContext.Connection.RemoteIpAddress}:{Request.HttpContext.Connection.RemotePort}";
           return await _weChatConfigServices.PushCardMsg(msg, pushUserIP);
        }
        /// <summary>
        /// 推送文本消息
        /// </summary>
        /// <param name="msg">消息对象</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<MessageModel<WeChatApiDto>> PushTxtMsg([FromBody] WeChatPushTestDto msg)
        {
            return await _weChatConfigServices.PushTxtMsg(msg);
        }
        /// <summary>
        /// 通过绑定用户获取微信用户信息(一般用于初次绑定检测)
        /// </summary>
        /// <param name="info">信息</param> 
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<WeChatResponseUserInfo>> GetBindUserInfo([FromQuery]WeChatUserInfo info)
        {
            return await _weChatConfigServices.GetBindUserInfo(info);
        }
        /// <summary>
        /// 用户解绑
        /// </summary>
        /// <param name="info">消息</param> 
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<WeChatResponseUserInfo>> UnBind([FromQuery]WeChatUserInfo info)
        {
            return await _weChatConfigServices.UnBind(info);
        }
    }
}
