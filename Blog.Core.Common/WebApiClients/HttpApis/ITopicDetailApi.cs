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
    /// Tibug 管理
    /// </summary>
    [TraceFilter]
    public interface ITopicDetailApi : IHttpApi
    {
        /// <summary>
        /// 删除 bug (Auth policies: Permission)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Success</returns>
        [HttpDelete("api/TopicDetail/Delete")]
        ITask<StringMessageModel> Delete5Async(int? id);

        /// <summary>
        /// 获取Bug数据列表（带分页）
        /// 【无权限】
        /// </summary>
        /// <param name="page">页数</param>
        /// <param name="tname">专题类型</param>
        /// <param name="key">关键字</param>
        /// <returns>Success</returns>
        [HttpGet("api/TopicDetail/Get")]
        ITask<TopicDetailPageModelMessageModel> Get9Async(int page, string tname, string key);

        /// <summary>
        /// 获取详情【无权限】
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Success</returns>
        [HttpGet("api/TopicDetail/Get/{id}")]
        ITask<TopicDetailMessageModel> Get10Async([Required] int id);

        /// <summary>
        /// 添加一个 BUG 【无权限】
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPost("api/TopicDetail/Post")]
        ITask<StringMessageModel> Post5Async([JsonContent] TopicDetail body);

        /// <summary>
        /// 更新 bug (Auth policies: Permission)
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPut("api/TopicDetail/Update")]
        ITask<StringMessageModel> Update2Async([JsonContent] TopicDetail body);

    }
}