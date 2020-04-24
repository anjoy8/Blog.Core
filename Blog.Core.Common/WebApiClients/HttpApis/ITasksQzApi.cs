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
    public interface ITasksQzApi : IHttpApi
    {
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="page"></param>
        /// <param name="key"></param>
        /// <returns>Success</returns>
        [HttpGet("api/TasksQz/Get")]
        ITask<TasksQzPageModelMessageModel> Get8Async(int page, string key);

        /// <summary>
        /// 添加计划任务
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPost("api/TasksQz/Post")]
        ITask<StringMessageModel> Post4Async([JsonContent] TasksQz body);

        /// <summary>
        /// 修改计划任务
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPut("api/TasksQz/Put")]
        ITask<StringMessageModel> Put4Async([JsonContent] TasksQz body);

        /// <summary>
        /// 重启一个计划任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns>Success</returns>
        [HttpGet("api/TasksQz/ReCovery")]
        ITask<StringMessageModel> ReCoveryAsync(System.Guid? jobId);

        /// <summary>
        /// 启动计划任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns>Success</returns>
        [HttpGet("api/TasksQz/StartJob")]
        ITask<StringMessageModel> StartJobAsync(int? jobId);

        /// <summary>
        /// 停止一个计划任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns>Success</returns>
        [HttpGet("api/TasksQz/StopJob")]
        ITask<StringMessageModel> StopJobAsync(int? jobId);

    }
}