using System;
using System.Collections.Generic;
using System.Linq;

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
    }
}
