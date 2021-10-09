using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Core.Model.ViewModels
{
    /// <summary>
    /// 微信推送消息Dto
    /// 作者:胡丁文
    /// 时间:2020-4-8 09:16:16
    /// </summary>
    public class WeChatCardMsgDataDto
    {
        /// <summary>
        /// 推送关键信息
        /// </summary>
        public WeChatUserInfo info { get; set; }
        /// <summary>
        /// 推送卡片消息Dto
        /// </summary>
        public WeChatCardMsgDetailDto cardMsg { set; get; }
    }
}
