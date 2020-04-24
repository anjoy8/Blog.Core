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
    /// 用户管理
    /// </summary>
    [TraceFilter]
    public interface IUserApi : IHttpApi
    {
        /// <summary>
        /// 删除用户 (Auth policies: Permission)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Success</returns>
        [HttpDelete("api/User/Delete")]
        ITask<StringMessageModel> Delete7Async(int? id);

        /// <summary>
        /// 获取全部用户 (Auth policies: Permission)
        /// </summary>
        /// <param name="page"></param>
        /// <param name="key"></param>
        /// <returns>Success</returns>
        [HttpGet("api/User/Get")]
        ITask<SysUserInfoPageModelMessageModel> Get13Async(int page, string key);

        /// <summary>
        /// (Auth policies: Permission)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Success</returns>
        [HttpGet("api/User/Get/{id}")]
        ITask<string> Get14Async([Required] string id);

        /// <summary>
        /// 获取用户详情根据token
        /// 【无权限】
        /// </summary>
        /// <param name="token">令牌</param>
        /// <returns>Success</returns>
        [HttpGet("api/User/GetInfoByToken")]
        ITask<SysUserInfoMessageModel> GetInfoByTokenAsync(string token);

        /// <summary>
        /// 添加一个用户 (Auth policies: Permission)
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPost("api/User/Post")]
        ITask<StringMessageModel> Post7Async([JsonContent] SysUserInfo body);

        /// <summary>
        /// 更新用户与角色 (Auth policies: Permission)
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPut("api/User/Put")]
        ITask<StringMessageModel> Put6Async([JsonContent] SysUserInfo body);

    }
}