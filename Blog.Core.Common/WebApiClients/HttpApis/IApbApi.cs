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
    public interface IApbApi : IHttpApi
    {
        /// <summary>
        /// (Auth policies: Permission)
        /// </summary>
        /// <returns>Success</returns>
        [HttpGet("api/V2/Apb/apbs")]
        ITask<List<string>> ApbsAsync();

    }
}