using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Blog.Core.Common.Helper
{
    /// <summary>
    /// httpclinet请求方式，请尽量使用IHttpClientFactory方式
    /// </summary>
    public class HttpHelper
    {
        public static readonly HttpClient Httpclient = new HttpClient();

        public static async Task<string> GetAsync(string serviceAddress)
        {
            try
            {
                string result = string.Empty;
                Uri getUrl = new Uri(serviceAddress);
                Httpclient.Timeout = new TimeSpan(0, 0, 60);
                result = await Httpclient.GetAsync(serviceAddress).Result.Content.ReadAsStringAsync();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }

        public static async Task<string> PostAsync(string serviceAddress, string requestJson = null)
        {
            try
            {
                string result = string.Empty;
                Uri postUrl = new Uri(serviceAddress);

                using (HttpContent httpContent = new StringContent(requestJson))
                {
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    Httpclient.Timeout = new TimeSpan(0, 0, 60);
                    result = await Httpclient.PostAsync(serviceAddress, httpContent).Result.Content.ReadAsStringAsync();
                }

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }
    }
}