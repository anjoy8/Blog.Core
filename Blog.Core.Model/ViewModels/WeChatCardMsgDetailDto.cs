namespace Blog.Core.Model.ViewModels
{
    /// <summary>
    /// 消息模板dto(如何填写数据,请参考微信模板即可)
    /// 作者:胡丁文
    /// 时间:2020-4-1 09:32:16
    /// </summary>
    public class WeChatCardMsgDetailDto
    {
        /// <summary>
        /// 消息模板
        /// </summary>
        public string template_id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string first { get; set; }
        /// <summary>
        /// 标题颜色(颜色代码都必须为#开头的16进制代码)
        /// </summary>
        public string colorFirst { get; set; } = "#173177";
        /// <summary>
        /// 内容1
        /// </summary>
        public string keyword1 { get; set; }
        /// <summary>
        /// 内容1颜色
        /// </summary>

        public string color1 { get; set; } = "#173177";
        /// <summary>
        /// 内容2
        /// </summary>
        public string keyword2 { get; set; }
        /// <summary>
        /// 内容2颜色
        /// </summary>
        public string color2 { get; set; } = "#173177";
        /// <summary>
        /// 内容3
        /// </summary>
        public string keyword3 { get; set; }
        /// <summary>
        /// 内容3颜色
        /// </summary>
        public string color3 { get; set; } = "#173177";
        /// <summary>
        /// 内容4
        /// </summary>
        public string keyword4 { get; set; }
        /// <summary>
        /// 内容4颜色
        /// </summary>
        public string color4 { get; set; } = "#173177";
        /// <summary>
        /// 内容5
        /// </summary>
        public string keyword5 { get; set; }
        /// <summary>
        /// 内容5颜色
        /// </summary>
        public string color5 { get; set; } = "#173177";
        /// <summary>
        /// 备注信息
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 备注信息颜色
        /// </summary>
        public string colorRemark { get; set; } = "#173177";
        /// <summary>
        /// 跳转连接
        /// </summary>
        public string url { get; set; }
    }
}
