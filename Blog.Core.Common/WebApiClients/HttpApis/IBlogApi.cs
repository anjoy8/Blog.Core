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
    /// 博客管理
    /// </summary>
    [TraceFilter]
    public interface IBlogApi : IHttpApi
    {
        /// <summary>
        /// 获取博客列表【无权限】
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="bcategory"></param>
        /// <param name="key"></param>
        /// <returns>Success</returns>
        [HttpGet("api/Blog")]
        ITask<BlogArticlePageModelMessageModel> BlogAsync(int? id, int page, string bcategory, string key);

        /// <summary>
        /// 添加博客【无权限】
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPost("api/Blog")]
        ITask<StringMessageModel> Blog2Async([JsonContent] BlogArticle body);

        /// <summary>
        /// 获取博客详情 (Auth)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Success</returns>
        [HttpGet("api/Blog/{id}")]
        ITask<BlogViewModelsMessageModel> Blog3Async([Required] int id);

        /// <summary>
        /// apache jemeter 压力测试
        /// 更新接口
        /// </summary>
        /// <returns>Success</returns>
        [HttpGet("api/Blog/ApacheTestUpdate")]
        ITask<BooleanMessageModel> ApacheTestUpdateAsync();

        /// <summary>
        /// 删除博客 (Auth policies: Permission)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Success</returns>
        [HttpDelete("api/Blog/Delete")]
        ITask<StringMessageModel> DeleteAsync(int? id);

        /// <summary>
        /// 获取详情【无权限】
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Success</returns>
        [HttpGet("api/Blog/DetailNuxtNoPer")]
        ITask<BlogViewModelsMessageModel> DetailNuxtNoPerAsync(int? id);

        /// <summary>
        /// 更新博客信息 (Auth)
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPut("api/Blog/Update")]
        ITask<StringMessageModel> UpdateAsync([JsonContent] BlogArticle body);

        /// <summary>
        /// 获取博客测试信息 v2版本
        /// </summary>
        /// <returns>Success</returns>
        [HttpGet("api/V2/Blog/Blogtest")]
        ITask<StringMessageModel> BlogtestAsync();

    }
}