using WebApiClient;
using WebApiClient.Attributes;

namespace Blog.Core.Common.WebApiClients.HttpApis
{
    /// <summary>
    /// 豆瓣视频管理
    /// </summary>
    [TraceFilter]
    public interface IDoubanApi : IHttpApi
    {
        /// <summary>
        /// 获取电影详情
        /// </summary>
        /// <param name="isbn"></param>
        [HttpGet("api/bookinfo")]
        ITask<DoubanViewModel> VideoDetailAsync(string isbn);

    }


}
