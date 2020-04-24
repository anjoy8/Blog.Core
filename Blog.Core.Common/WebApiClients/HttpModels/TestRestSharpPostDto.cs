using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace Blog.Core.Common.WebApiClients
{
    /// <summary>用来测试 RestSharp Post 请求</summary>
    public class TestRestSharpPostDto
    {
        [AliasAs("success")]
        public bool Success { get; set; }

        [AliasAs("name")]
        public string Name { get; set; }

    }
}