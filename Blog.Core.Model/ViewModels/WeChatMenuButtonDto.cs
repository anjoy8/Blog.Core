using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Core.Model.ViewModels
{
    /// <summary>
    /// 获取微信菜单DTO,用于存放具体菜单内容
    /// </summary>
    public class WeChatMenuButtonDto
    {
        public string type { get; set; }
        public string name { get; set; }
        public string key { get; set; }
        public string url { get; set; }
        public WeChatMenuButtonDto[] sub_button { get; set; }
    }
}
