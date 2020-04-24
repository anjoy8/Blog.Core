using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebApiClient;
using WebApiClient.Attributes;
using WebApiClient.DataAnnotations;
using WebApiClient.Parameterables;
namespace Blog.Core.Common.WebApiClients
{
    /// <summary>
    /// 类别管理【无权限】
    /// </summary>
    [TraceFilter]
    public interface ITopicApi : IHttpApi
    {
        /// <summary>
        /// 获取Tibug所有分类
        /// </summary>
        /// <returns>Success</returns>
        [HttpGet("api/Topic")]
        ITask<TopicListMessageModel> TopicAsync();

        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPost("api/Topic")]
        ITask<HttpResponseMessage> Topic2Async([JsonContent] string body);

        /// <param name="id"></param>
        /// <returns>Success</returns>
        [HttpGet("api/Topic/{id}")]
        ITask<string> Topic3Async([Required] int id);

        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPut("api/Topic/{id}")]
        ITask<HttpResponseMessage> Topic4Async([Required] int id, [JsonContent] string body);

        /// <param name="id"></param>
        /// <returns>Success</returns>
        [HttpDelete("api/Topic/{id}")]
        ITask<HttpResponseMessage> Topic5Async([Required] int id);

    }
}