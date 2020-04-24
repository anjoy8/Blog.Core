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
    [TraceFilter]
    public interface IDbFirstApi : IHttpApi
    {
        /// <summary>
        /// 获取 整体框架 文件 (Auth policies: Permission)
        /// </summary>
        /// <returns>Success</returns>
        [HttpGet("api/DbFirst/GetFrameFiles")]
        ITask<bool> GetFrameFilesAsync();

        /// <summary>
        /// 获取 IRepository 层文件 (Auth policies: Permission)
        /// </summary>
        /// <returns>Success</returns>
        [HttpGet("api/DbFirst/GetIRepositoryFiles")]
        ITask<bool> GetIRepositoryFilesAsync();

        /// <summary>
        /// 获取 IService 层文件 (Auth policies: Permission)
        /// </summary>
        /// <returns>Success</returns>
        [HttpGet("api/DbFirst/GetIServiceFiles")]
        ITask<bool> GetIServiceFilesAsync();

        /// <summary>
        /// 获取 Model 层文件 (Auth policies: Permission)
        /// </summary>
        /// <returns>Success</returns>
        [HttpGet("api/DbFirst/GetModelFiles")]
        ITask<bool> GetModelFilesAsync();

        /// <summary>
        /// 获取 Repository 层文件 (Auth policies: Permission)
        /// </summary>
        /// <returns>Success</returns>
        [HttpGet("api/DbFirst/GetRepositoryFiles")]
        ITask<bool> GetRepositoryFilesAsync();

        /// <summary>
        /// 获取 Services 层文件 (Auth policies: Permission)
        /// </summary>
        /// <returns>Success</returns>
        [HttpGet("api/DbFirst/GetServicesFiles")]
        ITask<bool> GetServicesFilesAsync();

    }
}