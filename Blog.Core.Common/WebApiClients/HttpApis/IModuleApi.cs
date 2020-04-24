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
    /// 接口管理
    /// </summary>
    [TraceFilter]
    public interface IModuleApi : IHttpApi
    {
        /// <summary>
        /// 删除一条接口 (Auth policies: Permission)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Success</returns>
        [HttpDelete("api/Module/Delete")]
        ITask<StringMessageModel> Delete2Async(int? id);

        /// <summary>
        /// 获取全部接口api (Auth policies: Permission)
        /// </summary>
        /// <param name="page"></param>
        /// <param name="key"></param>
        /// <returns>Success</returns>
        [HttpGet("api/Module/Get")]
        ITask<ModulePageModelMessageModel> GetAsync(int page, string key);

        /// <summary>
        /// (Auth policies: Permission)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Success</returns>
        [HttpGet("api/Module/Get/{id}")]
        ITask<string> Get2Async([Required] string id);

        /// <summary>
        /// 添加一条接口信息 (Auth policies: Permission)
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPost("api/Module/Post")]
        ITask<StringMessageModel> PostAsync([JsonContent] Module body);

        /// <summary>
        /// 更新接口信息 (Auth policies: Permission)
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPut("api/Module/Put")]
        ITask<StringMessageModel> PutAsync([JsonContent] Module body);

    }
}