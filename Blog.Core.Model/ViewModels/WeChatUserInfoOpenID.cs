namespace Blog.Core.Model.ViewModels
{
    /// <summary>
    /// 微信推送所需信息(OpenID版本)
    /// 作者:胡丁文
    /// 时间:2020-11-23 16:27:29
    /// </summary>
    public class WeChatUserInfoOpenID
    {
        /// <summary>
        /// 微信公众号ID
        /// </summary>
        public string id { get; set; } 
        /// <summary>
        /// 微信OpenID
        /// </summary>
        public List<string> userID { get; set; }
    }
}
