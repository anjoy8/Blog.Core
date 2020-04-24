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
    /// 角色管理
    /// </summary>
    [TraceFilter]
    public interface IRoleApi : IHttpApi
    {
        /// <summary>
        /// 删除角色 (Auth policies: Permission)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Success</returns>
        [HttpDelete("api/Role/Delete")]
        ITask<StringMessageModel> Delete4Async(int? id);

        /// <summary>
        /// 获取全部角色 (Auth policies: Permission)
        /// </summary>
        /// <param name="page"></param>
        /// <param name="key"></param>
        /// <returns>Success</returns>
        [HttpGet("api/Role/Get")]
        ITask<RolePageModelMessageModel> Get6Async(int page, string key);

        /// <summary>
        /// (Auth policies: Permission)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Success</returns>
        [HttpGet("api/Role/Get/{id}")]
        ITask<string> Get7Async([Required] string id);

        /// <summary>
        /// 添加角色 (Auth policies: Permission)
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPost("api/Role/Post")]
        ITask<StringMessageModel> Post3Async([JsonContent] Role body);

        /// <summary>
        /// 更新角色 (Auth policies: Permission)
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPut("api/Role/Put")]
        ITask<StringMessageModel> Put3Async([JsonContent] Role body);

    }
}