using Blog.Core.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Core.Model.ViewModels
{
    /// <summary>
    /// 用来测试 RestSharp Get 请求
    /// </summary>
    public class TestRestSharpGetDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string success { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public BlogArticle data { get; set; }
    }
}
