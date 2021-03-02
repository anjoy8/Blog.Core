using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Blog.Core.Common.Helper
{
    public class StringHelper
    {
        /// <summary>
        /// 根据分隔符返回前n条数据
        /// </summary>
        /// <param name="content">数据内容</param>
        /// <param name="separator">分隔符</param>
        /// <param name="top">前n条</param>
        /// <param name="isDesc">是否倒序（默认false）</param>
        /// <returns></returns>
        public static List<string> GetTopDataBySeparator(string content, string separator, int top, bool isDesc = false)
        {
            if (string.IsNullOrEmpty(content))
            {
                return new List<string>() { };
            }

            if (string.IsNullOrEmpty(separator))
            {
                throw new ArgumentException("message", nameof(separator));
            }

            var dataArray = content.Split(separator).Where(d => !string.IsNullOrEmpty(d)).ToArray();
            if (isDesc)
            {
                Array.Reverse(dataArray);
            }

            if (top > 0)
            {
                dataArray = dataArray.Take(top).ToArray();
            }

            return dataArray.ToList();
        }
        /// <summary>
        /// 根据字段拼接get参数
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static string GetPars(Dictionary<string, object> dic)
        {

            StringBuilder sb = new StringBuilder();
            string urlPars = null;
            bool isEnter = false;
            foreach (var item in dic)
            {
                sb.Append($"{(isEnter ? "&" : "")}{item.Key}={item.Value}");
                isEnter = true;
            }
            urlPars = sb.ToString();
            return urlPars;
        }
        /// <summary>
        /// 获取一个GUID
        /// </summary>
        /// <param name="format">格式-默认为N</param>
        /// <returns></returns>
        public static string GetGUID(string format="N") {
            return Guid.NewGuid().ToString(format);
        }
        /// <summary>  
        /// 根据GUID获取19位的唯一数字序列  
        /// </summary>  
        /// <returns></returns>  
        public static long GetGuidToLongID()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }
        /// <summary>
        /// 获取字符串最后X行
        /// </summary>
        /// <param name="resourceStr"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetCusLine(string resourceStr,int length) {
            string[] arrStr = resourceStr.Split("\r\n");
            return string.Join("", (from q in arrStr select q).Skip(arrStr.Length - length+1).Take(length).ToArray());  
        }

    }
}
