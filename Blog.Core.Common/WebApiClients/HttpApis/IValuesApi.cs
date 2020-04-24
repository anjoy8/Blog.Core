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
    /// Values控制器
    /// </summary>
    [TraceFilter]
    public interface IValuesApi : IHttpApi
    {
        /// <summary>
        /// route with template name.
        /// </summary>
        /// <returns>Success</returns>
        [HttpGet("api/custom/destination")]
        ITask<string> RouteAsync();

        /// <summary>
        /// to redirect by route template name.
        /// </summary>
        /// <returns>Success</returns>
        [HttpGet("api/custom/go-destination")]
        ITask<HttpResponseMessage> GoDestinationAsync();

        /// <summary>
        /// Get方法
        /// </summary>
        /// <returns>Success</returns>
        [HttpGet("api/Values")]
        ITask<ResponseEnumMessageModel> ValuesAsync();

        /// <summary>
        /// 测试 post 一个对象 + 独立参数
        /// </summary>
        /// <param name="id">独立参数</param>
        /// <param name="body">model实体类参数</param>
        /// <returns>Success</returns>
        [HttpPost("api/Values")]
        ITask<HttpResponseMessage> Values2Async(int? id, [JsonContent] BlogArticle body);

        /// <summary>
        /// Get(int id)方法
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Success</returns>
        [HttpGet("api/Values/{id}")]
        ITask<string> Values3Async([Required] int id);

        /// <summary>
        /// Put方法 (Auth)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPut("api/Values/{id}")]
        ITask<HttpResponseMessage> Values4Async([Required] int id, [JsonContent] string body);

        /// <summary>
        /// Delete方法 (Auth)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Success</returns>
        [HttpDelete("api/Values/{id}")]
        ITask<HttpResponseMessage> Values5Async([Required] int id);

        /// <summary>
        /// 测试参数是必填项 (Auth)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Success</returns>
        [HttpGet("api/values/RequiredPara")]
        ITask<string> RequiredParaAsync([Required] string id);

        /// <summary>
        /// 测试http请求 RestSharp Get
        /// </summary>
        /// <returns>Success</returns>
        [HttpGet("api/Values/RestsharpGet")]
        ITask<TestRestSharpGetDto> RestsharpGetAsync();

        /// <summary>
        /// 测试http请求 RestSharp Post
        /// </summary>
        /// <returns>Success</returns>
        [HttpGet("api/Values/RestsharpPost")]
        ITask<TestRestSharpPostDto> RestsharpPostAsync();

        /// <summary>
        /// 测试多库连接
        /// </summary>
        /// <returns>Success</returns>
        [HttpGet("api/Values/TestMutiDBAPI")]
        ITask<HttpResponseMessage> TestMutiDBAPIAsync();

        /// <summary>
        /// 测试 post 参数
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Success</returns>
        [HttpPost("api/Values/TestPostPara")]
        ITask<HttpResponseMessage> TestPostParaAsync(string name);

        /// <summary>
        /// 通过 HttpContext 获取用户信息 (Auth)
        /// </summary>
        /// <param name="claimType">声明类型，默认 jti</param>
        /// <returns>Success</returns>
        [HttpGet("api/values/UserInfo")]
        ITask<StringListMessageModel> UserInfoAsync([AliasAs("ClaimType")] string claimType);

    }
}