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
    /// 登录管理【无权限】
    /// </summary>
    [TraceFilter]
    public interface ILoginApi : IHttpApi
    {
        /// <summary>
        /// 获取JWT的方法2：给Nuxt提供
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pass"></param>
        /// <returns>Success</returns>
        [HttpGet("api/Login/GetTokenNuxt")]
        ITask<StringMessageModel> GetTokenNuxtAsync(string name, string pass);

        /// <summary>
        /// 获取JWT的方法4：给 JSONP 测试
        /// </summary>
        /// <param name="callBack"></param>
        /// <param name="id"></param>
        /// <param name="sub"></param>
        /// <param name="expiresSliding"></param>
        /// <param name="expiresAbsoulute"></param>
        /// <returns>Success</returns>
        [HttpGet("api/Login/jsonp")]
        ITask<HttpResponseMessage> JsonpAsync(string callBack, long id, string sub, int expiresSliding, int expiresAbsoulute);

        /// <summary>
        /// 获取JWT的方法3：整个系统主要方法
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pass"></param>
        /// <returns>Success</returns>
        [HttpGet("api/Login/JWTToken3.0")]
        ITask<TokenInfoViewModelMessageModel> JWTToken3_0Async(string name, string pass);

        /// <summary>
        /// 测试 MD5 加密字符串
        /// </summary>
        /// <param name="password"></param>
        /// <returns>Success</returns>
        [HttpGet("api/Login/Md5Password")]
        ITask<string> Md5PasswordAsync(string password);

        /// <summary>
        /// 请求刷新Token（以旧换新）
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Success</returns>
        [HttpGet("api/Login/RefreshToken")]
        ITask<TokenInfoViewModelMessageModel> RefreshTokenAsync(string token);

        /// <summary>
        /// 获取JWT的方法1
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pass"></param>
        /// <returns>Success</returns>
        [HttpGet("api/Login/Token")]
        ITask<StringMessageModel> TokenAsync(string name, string pass);

    }
}