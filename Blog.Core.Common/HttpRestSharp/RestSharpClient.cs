using RestSharp;
using RestSharp.Authenticators;
using System;

namespace Blog.Core.Common.HttpRestSharp
{
    /// <summary>
    /// Rest接口执行者
    /// </summary>
    public class RestSharpClient : IRestSharp
    {
        /// <summary>
        /// 请求客户端
        /// </summary>
        private RestClient client;

        /// <summary>
        /// 接口基地址 格式：http://apk.neters.club/
        /// </summary>
        private string BaseUrl { get; set; }

        /// <summary>
        /// 默认的时间参数格式
        /// </summary>
        private string DefaultDateParameterFormat { get; set; }

        /// <summary>
        /// 默认验证器
        /// </summary>
        private IAuthenticator DefaultAuthenticator { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="authenticator"></param>
        public RestSharpClient(string baseUrl, IAuthenticator authenticator = null)
        {
            BaseUrl = baseUrl;
            client = new RestClient(BaseUrl);
            DefaultAuthenticator = authenticator;

            //默认时间显示格式
            DefaultDateParameterFormat = "yyyy-MM-dd HH:mm:ss";

            //默认校验器
            if (DefaultAuthenticator != null)
            {
                client.Authenticator = DefaultAuthenticator;
            }
        }

        /// <summary>
        /// 通用执行方法
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <remarks>
        /// 调用实例：
        /// var client = new RestSharpClient("http://apk.neters.club/");
        /// var result = client.Execute(new RestRequest("v2/movie/in_theaters", Method.GET));
        /// var content = result.Content;//返回的字符串数据
        /// </remarks>
        /// <returns></returns>
        public IRestResponse Execute(IRestRequest request)
        {
            request.DateFormat = string.IsNullOrEmpty(request.DateFormat) ? DefaultDateParameterFormat : request.DateFormat;
            var response = client.Execute(request);
            return response;
        }

        /// <summary>
        /// 同步执行方法
        /// </summary>
        /// <typeparam name="T">返回的泛型对象</typeparam>
        /// <param name="request">请求参数</param>
        /// <remarks>
        ///  var client = new RestSharpClient("http://apk.neters.club/");
        ///  var result = client.Execute<List<string>>(new RestRequest("v2/movie/in_theaters", Method.GET)); 
        /// </remarks>
        /// <returns></returns>
        public T Execute<T>(IRestRequest request) where T : new()
        {
            request.DateFormat = string.IsNullOrEmpty(request.DateFormat) ? DefaultDateParameterFormat : request.DateFormat;
            var response = client.Execute<T>(request);
            return response.Data;
        }

        /// <summary>
        /// 异步执行方法
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <param name="callback">回调函数</param>
        /// <remarks>
        /// 调用实例：
        /// var client = new RestSharpClient("http://apk.neters.club/");
        /// client.ExecuteAsync<List<string>>(new RestRequest("v2/movie/in_theaters", Method.GET), result =>
        /// {
        ///      var content = result.Content;//返回的字符串数据
        /// });
        /// </remarks>
        /// <returns></returns>
        [Obsolete]
        public RestRequestAsyncHandle ExecuteAsync(IRestRequest request, Action<IRestResponse> callback)
        {
            request.DateFormat = string.IsNullOrEmpty(request.DateFormat) ? DefaultDateParameterFormat : request.DateFormat;
            return client.ExecuteAsync(request, callback);
        }

        /// <summary>
        /// 异步执行方法
        /// </summary>
        /// <typeparam name="T">返回的泛型对象</typeparam>
        /// <param name="request">请求参数</param>
        /// <param name="callback">回调函数</param>
        /// <remarks>
        /// 调用实例：
        /// var client = new RestSharpClient("http://apk.neters.club/");
        /// client.ExecuteAsync<List<string>>(new RestRequest("v2/movie/in_theaters", Method.GET), result =>
        /// {
        ///      if (result.StatusCode != HttpStatusCode.OK)
        ///      {
        ///         return;
        ///      }
        ///      var data = result.Data;//返回数据
        /// });
        /// </remarks>
        /// <returns></returns>
        [Obsolete]
        public RestRequestAsyncHandle ExecuteAsync<T>(IRestRequest request, Action<IRestResponse<T>> callback) where T : new()
        {
            request.DateFormat = string.IsNullOrEmpty(request.DateFormat) ? DefaultDateParameterFormat : request.DateFormat;
            return client.ExecuteAsync<T>(request, callback);
        }
    }
}
