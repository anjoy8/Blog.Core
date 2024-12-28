namespace Blog.Core.Model.ViewModels
{
    /// <summary>
    /// 微信keyword所需Dto
    /// 作者:胡丁文
    /// 时间:2020-4-8 09:18:08
    /// </summary>
    public class WeChatPushCardMsgValueColorDto
    {
        /// <summary>
        /// 内容
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 文字颜色
        /// </summary>
        public string color { get; set; } = "#173177";
    }
}
