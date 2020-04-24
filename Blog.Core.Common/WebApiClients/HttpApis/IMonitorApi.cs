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
    public interface IMonitorApi : IHttpApi
    {
        /// <summary>
        /// SignalR send data
        /// </summary>
        /// <returns>Success</returns>
        [HttpGet("api/Monitor/Get")]
        ITask<LogInfoListMessageModel> Get3Async();

        /// <returns>Success</returns>
        [HttpGet("api/Monitor/GetAccessApiByDate")]
        ITask<AccessApiDateViewMessageModel> GetAccessApiByDateAsync();

        /// <returns>Success</returns>
        [HttpGet("api/Monitor/GetAccessApiByHour")]
        ITask<AccessApiDateViewMessageModel> GetAccessApiByHourAsync();

        /// <returns>Success</returns>
        [HttpGet("api/Monitor/GetRequestApiinfoByWeek")]
        ITask<RequestApiWeekViewMessageModel> GetRequestApiinfoByWeekAsync();

    }
}