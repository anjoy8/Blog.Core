namespace Blog.Core.Model.ViewModels
{
    public class WeChatPushLinkMsgContentDto
    {
        /// <summary>
        /// 图文链接标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 图文描述
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 访问URL
        /// </summary>
        public string viewUrl { get; set; }
        /// <summary>
        /// 图片URL
        /// </summary>
        public string pictureUrl { get; set; } 
    }
}
