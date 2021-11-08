using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Core.Model.ViewModels
{
    /// <summary>
    /// 推送模拟消息Dto
    /// 作者:胡丁文
    /// 时间:2020-4-24 14:52:44
    /// </summary>
    public class WeChatPushTestDto
    {
        /// <summary>
        /// 当前选中的微信公众号
        /// </summary>
        public string selectWeChat { get; set; }
        /// <summary>
        /// 当前选中的操作集合
        /// </summary>
        public string selectOperate { get; set; }
        /// <summary>
        /// 当前选中的绑定还是订阅
        /// </summary>
        public string selectBindOrSub { get; set; }
        /// <summary>
        /// 当前选中的微信客户
        /// </summary>
        public string selectCompany { get; set; }
        /// <summary>
        /// 当前选中的消息类型
        /// </summary>
        public string selectMsgType { get; set; }
        /// <summary>
        /// 当前选中要发送的用户
        /// </summary>
        public string selectUser { get; set; }
        /// <summary>
        /// 文本消息
        /// </summary>
        public WeChatPushTextContentDto textContent { get; set; }
        /// <summary>
        /// 图片消息
        /// </summary>
        public WeChatPushPictureContentDto pictureContent { get; set; }
        /// <summary>
        /// 语音消息
        /// </summary>
        public WeChatPushVoiceContentDto voiceContent { get; set; }
        /// <summary>
        /// 视频消息
        /// </summary>
        public WeChatPushVideoContentDto videoContent { get; set; }
        /// <summary>
        /// 链接消息
        /// </summary>
        public WeChatPushLinkMsgContentDto linkMsgContent { get; set; }


    }
}
