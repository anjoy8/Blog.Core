using Blog.Core.Model.ViewModels;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Blog.Core.Common.Helper
{
    /// <summary>
    /// 微信公众号帮助类
    /// </summary>
    public static class WeChatHelper
    {
        /// <summary>
        /// 新增素材/上传多媒体文件(临时)
        /// http://mp.weixin.qq.com/wiki/5/963fc70b80dc75483a271298a76a8d59.html
        /// 1.上传的媒体文件限制：
        ///图片（image) : 1MB，支持JPG格式
        ///语音（voice）：1MB，播放长度不超过60s，支持MP4格式
        ///视频（video）：10MB，支持MP4格式
        ///缩略图（thumb)：64KB，支持JPG格式
        ///2.媒体文件在后台保存时间为3天，即3天后media_id失效
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="type">媒体文件类型，分别有图片（image）、语音（voice）、视频（video）和缩略图（thumb）</param>
        /// <param name="fileName">文件名</param>
        /// <param name="inputStream">文件输入流</param>
        /// <returns>media_id</returns>
        public async static Task<WeChatApiDto> UploadMediaTemp(string token, string type, string fileName, Stream inputStream)
        {
            var url = $"http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token={token}&type={type}";
            using var client = new HttpClient();
            using HttpContent content = new StreamContent(inputStream);
            var httpResponse = await client.PostAsync(url, content);
            var txt = await httpResponse.Content.ReadAsStringAsync();
            var data = JsonHelper.ParseFormByJson<WeChatApiDto>(txt);
            return data;
        }
        /// <summary>
        /// 新增素材/上传多媒体文件(永久)
        /// http://mp.weixin.qq.com/wiki/5/963fc70b80dc75483a271298a76a8d59.html
        /// 1.上传的媒体文件限制：
        ///图片（image) : 1MB，支持JPG格式
        ///语音（voice）：1MB，播放长度不超过60s，支持MP4格式
        ///视频（video）：10MB，支持MP4格式
        ///缩略图（thumb)：64KB，支持JPG格式 
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="type">媒体文件类型，分别有图片（image）、语音（voice）、视频（video）和缩略图（thumb）</param>
        /// <param name="fileName">文件名</param>
        /// <param name="inputStream">文件输入流</param>
        /// <returns>media_id</returns>
        public async static Task<WeChatApiDto> UploadMedia(string token, string type, string fileName, Stream inputStream)
        {
            var url = $"https://api.weixin.qq.com/cgi-bin/material/add_material?access_token={token}&type={type}"; 
            using var client = new HttpClient();
            using HttpContent content = new StreamContent(inputStream);
            var httpResponse = await client.PostAsync(url, content);
            var txt = await httpResponse.Content.ReadAsStringAsync();
            var data = JsonHelper.ParseFormByJson<WeChatApiDto>(txt);
            return data;
        }
        /// <summary>
        /// 通过绑定票据获取公众号关注二维码
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public async static Task<WeChatApiDto> GetQRCodePicture(string ticket)
        {
            string url = $"https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={ticket}"; 
            var txt = await HttpHelper.GetAsync(url);
            var data = JsonHelper.ParseFormByJson<WeChatApiDto>(txt);
            return data;
        }
        /// <summary>
        /// 获取临时关注二维码
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="postData">The post data.</param>
        public async static Task<WeChatApiDto> GetQRCode(string token, string jsonData)
        {
            string url = $"https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={token}";
            var txt = await HttpHelper.PostAsync(url, jsonData);
            var data = JsonHelper.ParseFormByJson<WeChatApiDto>(txt);
            return data;
        }
        /// <summary>
        /// 获取关注的公众号用户openid(获取所有OpenID)
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="isGetAll">是否递归获取所有用户的</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        public async static Task<WeChatApiDto> GetUsers(string token,bool isGetAll=false)
        { 
            string url = $"https://api.weixin.qq.com/cgi-bin/user/get?access_token={token}"; 
            var txt = await HttpHelper.GetAsync(url);
            var data = JsonHelper.ParseFormByJson<WeChatApiDto>(txt);
            if (data.data == null) data.data = new WeChatOpenIDsDto();
            if(!string.IsNullOrEmpty(data.next_openid))
                await GetUsers(token, data.next_openid, data.data.openid);
            return data; 
        }
        /// <summary>
        /// 获取关注的公众号用户openid(递归)
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="nextUser">The next user.</param>
        /// <param name="users">The users.</param>
        public async static Task GetUsers(string token, string nextUser, List<string> users)
        {
            string url = $"https://api.weixin.qq.com/cgi-bin/user/get?access_token={token}&next_openid={nextUser}";
            var txt = await HttpHelper.GetAsync(url);
            var data = JsonHelper.ParseFormByJson<WeChatApiDto>(txt);
            if (data.data != null && data.data.openid != null)
                users.AddRange(data.data.openid);
            if (!string.IsNullOrEmpty(data.next_openid))
                await GetUsers(token, data.next_openid, data.data.openid);
        }
        /// <summary>
        /// 获取菜单内容(获取菜单有menu外层,提交菜单不需要menu外层)
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>System.String.</returns>
        public async static Task<WeChatApiDto> GetMenu(string token)
        {
            string url = $"https://api.weixin.qq.com/cgi-bin/menu/get?access_token={token}";  
            var txt = await HttpHelper.GetAsync(url);
            var data = JsonHelper.ParseFormByJson<WeChatApiDto>(txt);
            return data;
        }
        /// <summary>
        /// 转换微信菜单按钮为事件的按钮
        /// </summary>
        public static void ConverMenuButtonForEvent(WeChatApiDto weChatApiDto)
        {
            foreach (var item in weChatApiDto?.menu?.button)
            {
                if (item.key.ObjToString().Equals("event") || item.type.ObjToString().Equals("event"))
                {
                    var temp = item.type;
                    item.type = item.key;
                    item.key = temp;
                } 
                if (item.sub_button != null)
                {
                    ConverMenuButtonForEvent(item.sub_button);
                }
            }
        }
        /// <summary>
        /// 转换微信菜单按钮为事件的按钮
        /// </summary>
        public static void ConverMenuButtonForEvent(WeChatMenuButtonDto[] weChatMenuButtonDto)
        {
            foreach (var item in weChatMenuButtonDto)
            {
                if (item.key.ObjToString().Equals("event") || item.type.ObjToString().Equals("event"))
                {
                    var temp = item.type;
                    item.type = item.key;
                    item.key = temp;
                }
                if (item.sub_button != null)
                {
                    ConverMenuButtonForEvent(item.sub_button);
                }
            }
        }
        /// <summary>
        /// 设置菜单内容(设置菜单不需要menu外层)
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="jsonMenu">The json menu.</param>
        /// <returns>System.String.</returns>
        public async static Task<WeChatApiDto> SetMenu(string token, string jsonMenu)
        {
            string url = $"https://api.weixin.qq.com/cgi-bin/menu/create?access_token={token}";
            var txt = await HttpHelper.PostAsync(url, jsonMenu);
            var data = JsonHelper.ParseFormByJson<WeChatApiDto>(txt);
            return data;
        }
        /// <summary>
        /// 删除菜单内容
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public async static Task<WeChatApiDto> DeleteMenu(string token)
        {
            string url = $"https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={token}"; 
            var txt = await HttpHelper.GetAsync(url);
            var data = JsonHelper.ParseFormByJson<WeChatApiDto>(txt);
            return data;
        }
        /// <summary>
        /// 发送普通消息(群发所有人,单人发送也可以)
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="jsonData">The json data.</param>
        /// <returns>System.String.</returns>
        public async static Task<WeChatApiDto> SendMsgToAll(string token, string jsonData)
        {
            string url = $"https://api.weixin.qq.com/cgi-bin/message/mass/sendall?access_token={token}";
            var txt = await HttpHelper.PostAsync(url, jsonData);
            var data = JsonHelper.ParseFormByJson<WeChatApiDto>(txt);
            return data;
        }
        /// <summary>
        /// 发送普通消息(单个人-24小时内用户跟微信公众号有互动才会推送成功)
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="jsonData">The json data.</param>
        /// <returns>System.String.</returns>
        public async static Task<WeChatApiDto> SendMsg(string token, string jsonData)
        {
            string url = $"https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={token}";
            var txt = await HttpHelper.PostAsync(url, jsonData);
            var data = JsonHelper.ParseFormByJson<WeChatApiDto>(txt);
            return data;
        }
        /// <summary>
        /// 发送卡片消息模板
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="jsonData">The json data.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public async static Task<WeChatApiDto> SendCardMsg(string token, string jsonData)
        {
            string url = $"https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={token}";
            var txt = await HttpHelper.PostAsync(url, jsonData);
            var data = JsonHelper.ParseFormByJson<WeChatApiDto>(txt);
            return data;
        }
        /// <summary>
        /// 拉取普通access_token
        /// </summary>
        /// <param name="appid">appid</param>
        /// <param name="appsecret">appsecret</param>
        /// <returns>返回token</returns>
        public async static Task<WeChatApiDto> GetToken(string appid, string appsecret)
        { 
            string url = $"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={appid}&secret={appsecret}";
            var txt = await HttpHelper.GetAsync(url);
            var data = JsonHelper.ParseFormByJson<WeChatApiDto>(txt);
            return data;
        }
        /// <summary>
        /// 获取微信服务器IP列表
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>System.String.</returns>
        public async static Task<WeChatApiDto> GetWechatIP(string token)
        {
            string url = $"https://api.weixin.qq.com/cgi-bin/getcallbackip?access_token={token}";
            var txt = await HttpHelper.GetAsync(url);
            var data = JsonHelper.ParseFormByJson<WeChatApiDto>(txt);
            return data;
        }
        /// <summary>
        /// openid获取微信用户信息 
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="openid">The openid.</param>
        /// <returns>Dictionary&lt;System.String, System.Object&gt;.</returns>
        public async static Task<WeChatApiDto> GetUserInfo(string token,string openid)
        {
            string url = $"https://api.weixin.qq.com/cgi-bin/user/info?access_token={token}&openid={openid}&lang=zh_CN";
            var txt = await HttpHelper.GetAsync(url);
            var data = JsonHelper.ParseFormByJson<WeChatApiDto>(txt);
            return data;
        }
        /// <summary>
        /// openid获取微信用户信息
        /// </summary>
        /// <param name="token">The openid.</param>
        /// <param name="openid">The access token.</param>
        public async static Task<WeChatApiDto> GetUserInfoTwo(string token,string openid)
        {
            string url = $"https://api.weixin.qq.com/sns/userinfo?access_token={token}&openid={openid}&lang=zh_CN"; 
            var txt = await HttpHelper.GetAsync(url);
            var data = JsonHelper.ParseFormByJson<WeChatApiDto>(txt);
            return data;
        }
        /// <summary>
        /// code换取用户openID
        /// </summary>
        /// <param name="appid">The appid.</param>
        /// <param name="appsecret">The appsecret.</param>
        /// <param name="code">The code.</param>
        /// <returns>Dictionary&lt;System.String, System.Object&gt;.</returns>
        public async static Task<WeChatApiDto> GetOpenidByCode(string appid, string appsecret, string code)
        {
            string url = $"https://api.weixin.qq.com/sns/oauth2/access_token?appid={appid}&secret={appsecret}&code={code}&grant_type=authorization_code";
            var txt = await HttpHelper.GetAsync(url);
            var data = JsonHelper.ParseFormByJson<WeChatApiDto>(txt);
            return data;
        }
        /// <summary>
        /// 获取模板消息
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>Dictionary&lt;System.String, System.Object&gt;.</returns>
        public async static Task<WeChatApiDto> GetTemplate(string token)
        {
            string url = $"https://api.weixin.qq.com/cgi-bin/template/get_all_private_template?access_token={token}";
            var txt = await HttpHelper.GetAsync(url);
            var data = JsonHelper.ParseFormByJson<WeChatApiDto>(txt);
            return data;
        } 
    }
}
