using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Blog.Core.Common.HttpRestSharp
{
    /// <summary>
    /// 基于 RestSharp 封装HttpHelper
    /// </summary>
    public static class HttpHelper
    {
        /// <summary>
        /// Get 请求
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="baseUrl">根域名:http://apk.neters.club/</param>
        /// <param name="url">接口:api/xx/yy</param>
        /// <param name="pragm">参数:id=2&name=老张</param>
        /// <returns></returns>
        public static T GetApi<T>(string baseUrl, string url, string pragm = "")
        {
            var client = new RestSharpClient(baseUrl);

            var request = client.Execute(string.IsNullOrEmpty(pragm)
                ? new RestRequest(url, Method.GET)
                : new RestRequest($"{url}?{pragm}", Method.GET));

            if (request.StatusCode != HttpStatusCode.OK)
            {
                return (T)Convert.ChangeType(request.ErrorMessage, typeof(T));
            }

            dynamic temp = Newtonsoft.Json.JsonConvert.DeserializeObject(request.Content, typeof(T));

            //T result = (T)Convert.ChangeType(request.Content, typeof(T));

            return (T)temp;
        }

        /// <summary>
        /// Post 请求
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="url">完整的url</param>
        /// <param name="body">post body,可以匿名或者反序列化</param>
        /// <returns></returns>
        public static T PostApi<T>(string url, object body = null)
        {
            var client = new RestClient($"{url}");
            IRestRequest queest = new RestRequest();
            queest.Method = Method.POST;
            queest.AddHeader("Accept", "application/json");
            queest.RequestFormat = DataFormat.Json;
            queest.AddBody(body); // 可以使用 JsonSerializer
            var result = client.Execute(queest);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                return (T)Convert.ChangeType(result.ErrorMessage, typeof(T));
            }

            dynamic temp = Newtonsoft.Json.JsonConvert.DeserializeObject(result.Content, typeof(T));

            //T result = (T)Convert.ChangeType(request.Content, typeof(T));

            return (T)temp;
        }
    }
}
