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
    public class CustomResultHandler : DelegatingHandler
    {
        JsonSerializerSettings _camelSettings = new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() };

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);
            var contentType = response.Content.Headers.ContentType?.MediaType ?? "";
            if (!contentType.Equals("application/json")) return response;

            dynamic result = null;
            var resultStr = await response.Content.ReadAsStringAsync();
            try
            {
                Console.WriteLine(resultStr);
                result = JsonConvert.DeserializeObject<dynamic>(resultStr);
            }
            catch (Exception)
            {
                return response;
            }

            if (result != null && result.errorCode == 500) resultStr = result.message.ToString();

            var exception = new Exception(resultStr);

            if (response.StatusCode == HttpStatusCode.InternalServerError || result.errorCode == (int)HttpStatusCode.InternalServerError)
            {
                var apiResult = new
                {
                    Result = false,
                    Message = "服务器内部错误",
                    ErrorCode = (int)HttpStatusCode.InternalServerError,
                    Data = new
                    {
                        exception.Message,
                        exception.StackTrace
                    }
                };
                response.Content = new StringContent(JsonConvert.SerializeObject(apiResult, _camelSettings), Encoding.UTF8, "application/json");
            }
            else
            {

            }


            return response;
        }
    }
}
