using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Core.Model.ViewModels
{
    /// <summary>
    /// 用来测试 RestSharp Post 请求
    /// </summary>
    public class TestRestSharpPostDto
    {
        public bool success { get; set; }
        public string name { get; set; }
    }
}
