using Blog.Core.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Core.Gateway.Extensions
{
    /// <summary>
    /// 这里不需要，目前集成的是 Blog.Core.Extensions 下的接口处理器
    /// 但是你可以单独在网关中使用这个。
    /// </summary>
    public class ApiResponseHandler : DelegatingHandler
    {
        JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);
            var contentType = response.Content.Headers.ContentType?.MediaType ?? "";
            if (!contentType.Equals("application/json")) return response;

            dynamic result = null;
            var resultStr = await response.Content.ReadAsStringAsync();
            try
            {
                result = JsonConvert.DeserializeObject<dynamic>(resultStr);
            }
            catch (Exception)
            {
                return response;
            }

            if (result != null && result.code == 500) resultStr = result.msg.ToString();

            var apiResponse = new ApiResponse(StatusCode.CODE200).MessageModel;
            if (response.StatusCode != HttpStatusCode.OK || result.code == (int)HttpStatusCode.InternalServerError)
            {
                var exception = new Exception(resultStr);
                apiResponse = new ApiResponse(StatusCode.CODE500).MessageModel;
            }
            else if (result.code == (int)HttpStatusCode.Unauthorized)
            {
                apiResponse = new ApiResponse(StatusCode.CODE401).MessageModel;

            }
            else if (result.code == (int)HttpStatusCode.Forbidden)
            {
                apiResponse = new ApiResponse(StatusCode.CODE403).MessageModel;

            }
            else
            {

            }

            var statusCode = apiResponse.status == 500 ? HttpStatusCode.InternalServerError
                : apiResponse.status == 401 ? HttpStatusCode.Unauthorized
                : apiResponse.status == 403 ? HttpStatusCode.Forbidden
                : HttpStatusCode.OK;

            response.StatusCode = statusCode;
            response.Content = new StringContent(JsonConvert.SerializeObject(apiResponse, jsonSerializerSettings), Encoding.UTF8, "application/json");

            return response;
        }
    }


}
