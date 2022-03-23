namespace Blog.Core.Model.ViewModels
{
    /// <summary>
    /// 微信消息模板Dto
    /// </summary>
    public class WeChatTemplateList
    {
        public string template_id { get; set; }
        public string title { get; set; }
        public string primary_industry { get; set; }
        public string deputy_industry { get; set; }
        public string content { get; set; }
        public string example { get; set; }
    }
}
