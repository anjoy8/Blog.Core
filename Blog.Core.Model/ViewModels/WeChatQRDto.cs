namespace Blog.Core.Model.ViewModels
{
    /// <summary>
    /// 微信二维码预装信息DTO
    /// </summary>
    public class WeChatQRDto
    {
        public int expire_seconds { get; set; }
        public string action_name { get; set; }
        public WeChatQRActionDto action_info { get; set; }
    }
}
