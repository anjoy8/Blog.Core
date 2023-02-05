using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Common.Helper
{
    /// <summary>
    /// Linq扩展
    /// </summary>
    public static class ExpressionExtensions_Nacos
    {
        #region Nacos NamingService

        private static readonly HttpClient httpclient = new HttpClient();

        private static string GetServiceUrl(Nacos.V2.INacosNamingService serv, string ServiceName, string Group, string apiurl)
        {
            try
            {
                var instance = serv.SelectOneHealthyInstance(ServiceName, Group).GetAwaiter().GetResult();
                var host = $"{instance.Ip}:{instance.Port}";
                if (instance.Metadata.ContainsKey("endpoint")) host = instance.Metadata["endpoint"];


                var baseUrl = instance.Metadata.TryGetValue("secure", out _)
                    ? $"https://{host}"
                    : $"http://{host}";

                if (string.IsNullOrWhiteSpace(baseUrl))
                {
                    return "";
                }

                return $"{baseUrl}{apiurl}";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return "";
        }

        public static async Task<string> Cof_NaoceGet(this Nacos.V2.INacosNamingService serv, string ServiceName, string Group, string apiurl, Dictionary<string, string> Parameters = null)
        {
            try
            {
                var url = GetServiceUrl(serv, ServiceName, Group, apiurl);
                if (string.IsNullOrEmpty(url)) return "";
                if (Parameters != null && Parameters.Any())
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var pitem in Parameters)
                    {
                        sb.Append($"{pitem.Key}={pitem.Value}&");
                    }

                    url = $"{url}?{sb.ToString().Trim('&')}";
                }

                httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var result = await httpclient.GetAsync(url);
                return await result.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return "";
        }

        public static async Task<string> Cof_NaocePostForm(this Nacos.V2.INacosNamingService serv, string ServiceName, string Group, string apiurl, Dictionary<string, string> Parameters)
        {
            try
            {
                var url = GetServiceUrl(serv, ServiceName, Group, apiurl);
                if (string.IsNullOrEmpty(url)) return "";

                var content = (Parameters != null && Parameters.Any()) ? new FormUrlEncodedContent(Parameters) : null;
                httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var result = await httpclient.PostAsync(url, content);
                return await result.Content.ReadAsStringAsync(); //.GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return "";
        }

        public static async Task<string> Cof_NaocePostJson(this Nacos.V2.INacosNamingService serv, string ServiceName, string Group, string apiurl, string jSonData)
        {
            try
            {
                var url = GetServiceUrl(serv, ServiceName, Group, apiurl);
                if (string.IsNullOrEmpty(url)) return "";
                httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var result = await httpclient.PostAsync(url, new StringContent(jSonData, Encoding.UTF8, "application/json"));
                return await result.Content.ReadAsStringAsync(); //.GetAwaiter().GetResult();

                //httpClient.BaseAddress = new Uri("https://www.testapi.com");
                //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return "";
        }

        public static async Task<string> Cof_NaocePostFile(this Nacos.V2.INacosNamingService serv, string ServiceName, string Group, string apiurl, Dictionary<string, byte[]> Parameters)
        {
            try
            {
                var url = GetServiceUrl(serv, ServiceName, Group, apiurl);
                if (string.IsNullOrEmpty(url)) return "";

                var content = new MultipartFormDataContent();
                foreach (var pitem in Parameters)
                {
                    content.Add(new ByteArrayContent(pitem.Value), "files", pitem.Key);
                }

                httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var result = await httpclient.PostAsync(url, content);
                return await result.Content.ReadAsStringAsync(); //.GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                //InfluxdbHelper.GetInstance().AddLog("Cof_NaocePostFile.Err", ee);
                Console.WriteLine(e.Message);
            }

            return "";
        }

        #endregion
    }

}
