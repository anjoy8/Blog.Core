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
    public interface ITransactionApi : IHttpApi
    {
        /// <param name="id"></param>
        /// <returns>Success</returns>
        [HttpDelete("api/Transaction/Delete/{id}")]
        ITask<bool> Delete6Async([Required] int id);

        /// <returns>Success</returns>
        [HttpGet("api/Transaction/Get")]
        ITask<StringIEnumerableMessageModel> Get11Async();

        /// <param name="id"></param>
        /// <returns>Success</returns>
        [HttpGet("api/Transaction/Get/{id}")]
        ITask<bool> Get12Async([Required] int id);

        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPost("api/Transaction/Post")]
        ITask<HttpResponseMessage> Post6Async([JsonContent] string body);

        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPut("api/Transaction/Put/{id}")]
        ITask<HttpResponseMessage> Put5Async([Required] int id, [JsonContent] string body);

    }
}