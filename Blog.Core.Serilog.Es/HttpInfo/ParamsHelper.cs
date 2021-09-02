using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web;

namespace Blog.Core.Serilog.Es.HttpInfo
{
    /// <summary>
    /// 获取参数帮助类
    /// </summary>
    public class ParamsHelper
    {
        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetParams(HttpContext context)
        {
            try
            {
                NameValueCollection form = HttpUtility.ParseQueryString(context.Request.QueryString.ToString());
                HttpRequest request = context.Request;

                string data = string.Empty;
                switch (request.Method)
                {
                    case "POST":

                        request.Body.Position = 0;
                        using (var ms = new MemoryStream())
                        {
                            request.Body.CopyTo(ms);
                            var b = ms.ToArray();
                            data = Encoding.UTF8.GetString(b); //把body赋值给bodyStr
                            
                        }
                        break;
                    case "GET":
                        //第一步：取出所有get参数
                        IDictionary<string, string> parameters = new Dictionary<string, string>();
                        for (int f = 0; f < form.Count; f++)
                        {
                            string key = form.Keys[f];
                            parameters.Add(key, form[key]);
                        }

                        // 第二步：把字典按Key的字母顺序排序
                        IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters);
                        IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

                        // 第三步：把所有参数名和参数值串在一起
                        StringBuilder query = new StringBuilder();
                        while (dem.MoveNext())
                        {
                            string key = dem.Current.Key;
                            string value = dem.Current.Value;
                            if (!string.IsNullOrEmpty(key))
                            {
                                query.Append(key).Append("=").Append(value).Append("&");
                            }
                        }
                        data = query.ToString().TrimEnd('&');
                        break;
                    default:
                        data = string.Empty;

                        break;
                }
                return data;
            }
            catch(Exception ex)
            {
                return string.Empty;
            }
        }

    }
}
