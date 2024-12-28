namespace Blog.Core.Model.ViewModels
{
    /// <summary>
    /// 推送给微信所需Dto
    /// 作者:胡丁文
    /// 时间:2020-4-8 09:16:16
    /// </summary>
    public class WeChatPushCardMsgDto
    {
        /// <summary>
        /// 推送微信用户ID
        /// </summary>
        public string touser { get; set; }
        /// <summary>
        /// 推送的模板ID
        /// </summary>
        public string template_id { get; set; }
        /// <summary>
        /// 推送URL地址
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 推送的数据
        /// </summary>
        public WeChatPushCardMsgDetailDto data { get; set; }
    }
}
