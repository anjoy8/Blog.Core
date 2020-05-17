using Blog.Core.Model;
using Blog.Core.Model.Models;
using Blog.Core.Model.ViewModels;
using System.ComponentModel.DataAnnotations;
using WebApiClient;
using WebApiClient.Attributes;

namespace Blog.Core.Common.WebApiClients.HttpApis
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
        ITask<MessageModel<PageModel<BlogArticle>>> BlogAsync(int? id, int page, string bcategory, string key);

        /// <summary>
        /// 添加博客【无权限】
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPost("api/Blog")]
        ITask<MessageModel<string>> Blog2Async([JsonContent] BlogArticle body);

        /// <summary>
        /// 获取博客详情 (Auth)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Success</returns>
        [HttpGet("api/Blog/{id}")]
        ITask<MessageModel<BlogViewModels>> Blog3Async([Required] int id);

        /// <summary>
        /// apache jemeter 压力测试
        /// 更新接口
        /// </summary>
        /// <returns>Success</returns>
        [HttpGet("api/Blog/ApacheTestUpdate")]
        ITask<MessageModel<bool>> ApacheTestUpdateAsync();

        /// <summary>
        /// 删除博客 (Auth policies: Permission)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Success</returns>
        [HttpDelete("api/Blog/Delete")]
        ITask<MessageModel<string>> DeleteAsync(int? id);

        /// <summary>
        /// 获取详情【无权限】
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Success</returns>
        [HttpGet("api/Blog/DetailNuxtNoPer")]
        ITask<MessageModel<BlogViewModels>> DetailNuxtNoPerAsync(int? id);

        /// <summary>
        /// 更新博客信息 (Auth)
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPut("api/Blog/Update")]
        ITask<MessageModel<string>> UpdateAsync([JsonContent] BlogArticle body);

        /// <summary>
        /// 获取博客测试信息 v2版本
        /// </summary>
        /// <returns>Success</returns>
        [HttpGet("api/V2/Blog/Blogtest")]
        ITask<MessageModel<string>> BlogtestAsync();

    }
}
