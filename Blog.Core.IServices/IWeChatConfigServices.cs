using Blog.Core.IServices.BASE;
using Blog.Core.Model;
using Blog.Core.Model.Models;
using Blog.Core.Model.ViewModels;
using System.Threading.Tasks;

namespace Blog.Core.IServices
{	
	/// <summary>
	/// IWeChatConfigServices
	/// </summary>	
    public interface IWeChatConfigServices :IBaseServices<WeChatConfig>
	{
		/// <summary>
		/// 获取可用的微信token
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<MessageModel<WeChatApiDto>> GetToken(string id);
		/// <summary>
		/// 刷新微信token
		/// </summary>
		/// <param name="publicAccount"></param>
		/// <returns></returns>
		Task<MessageModel<WeChatApiDto>> RefreshToken(string id);
		/// <summary>
		/// 获取模板信息
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<MessageModel<WeChatApiDto>> GetTemplate(string id);
		/// <summary>
		/// 获取菜单
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<MessageModel<WeChatApiDto>> GetMenu(string id);
		/// <summary>
		/// 获取订阅用户
		/// </summary>
		/// <param name="id"></param>
		/// <param name="openid"></param>
		/// <returns></returns>
		Task<MessageModel<WeChatApiDto>> GetSubUser(string id,string openid);
		/// <summary>
		/// 获取订阅用户列表
		/// </summary>
		/// <returns></returns>
		Task<MessageModel<WeChatApiDto>> GetSubUsers(string id);
		/// <summary>
		/// 处理微信事件
		/// </summary>
		/// <param name="weChat"></param>
		/// <returns></returns>
		Task<string> HandleWeChat(WeChatXMLDto weChat);
		/// <summary>
		/// 微信验证入库
		/// </summary>
		/// <param name="validDto"></param>
		/// <param name="body"></param>
		/// <returns></returns>

		Task<string> Valid(WeChatValidDto validDto,string body);
		/// <summary>
		/// 获取绑定二维码
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		Task<MessageModel<WeChatResponseUserInfo>> GetQRBind(WeChatUserInfo info);
		/// <summary>
		/// 推送卡片消息(绑定用户)
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="ip"></param>
		/// <returns></returns>
		Task<MessageModel<WeChatResponseUserInfo>> PushCardMsg(WeChatCardMsgDataDto msg,string ip);
		/// <summary>
		/// 推送文本消息(绑定或订阅)
		/// </summary>
		/// <param name="msg"></param>
		/// <returns></returns>
		Task<MessageModel<WeChatApiDto>> PushTxtMsg(WeChatPushTestDto msg);
		/// <summary>
		/// 更新菜单
		/// </summary>
		/// <param name="menu"></param>
		/// <returns></returns>
		Task<MessageModel<WeChatApiDto>> UpdateMenu(WeChatApiDto menu);
		/// <summary>
		/// 通过绑定用户获取微信用户信息
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		Task<MessageModel<WeChatResponseUserInfo>> GetBindUserInfo(WeChatUserInfo info);
		/// <summary>
		/// 解除绑定用户
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		Task<MessageModel<WeChatResponseUserInfo>> UnBind(WeChatUserInfo info);
	}
}