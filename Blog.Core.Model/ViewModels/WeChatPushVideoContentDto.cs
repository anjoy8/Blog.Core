namespace Blog.Core.Model.ViewModels
{
    public class WeChatPushVideoContentDto
    {
        /// <summary>
        /// 视频标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 视频封面mediaID
        /// </summary>
        public string pictureMediaID { get; set; }
        /// <summary>
        /// 视频mediaID
        /// </summary>
        public string videoMediaID { get; set; }
    }
}
