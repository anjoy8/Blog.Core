using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace Blog.Core.Common.WebApiClients
{
    /// <summary>用来测试 RestSharp Get 请求</summary>
    public class TestRestSharpGetDto
    {
        [AliasAs("success")]
        public string Success { get; set; }

        [AliasAs("data")]
        public BlogArticle Data { get; set; }

    }
}