using System.Collections.Generic;

namespace Blog.Core.Common.LogHelper
{
    public class ApiWeek
    {
        public string week { get; set; }
        public string url { get; set; }
        public int count { get; set; }
    }
    public class ApiDate
    {
        public string date { get; set; }
        public int count { get; set; }
    }

    public class ActiveUserVM
    {
        public string user { get; set; }
        public int count { get; set; }
    }

    public class RequestApiWeekView
    {
        public List<string> columns { get; set; }
        public string rows { get; set; }
    }
    public class AccessApiDateView
    {
        public string[] columns { get; set; }
        public List<ApiDate> rows { get; set; }
    }
    public class RequestInfo
    {
        public string Ip { get; set; }
        public string Url { get; set; }
        public string Datetime { get; set; }
        public string Date { get; set; }
        public string Week { get; set; }

    }

    public class ApiLogAopInfo
    {
        /// <summary>
        /// 请求时间
        /// </summary>
        public string RequestTime { get; set; } = string.Empty;
        /// <summary>
        /// 操作人员
        /// </summary>
        public string OpUserName { get; set; } = string.Empty;
        /// <summary>
        /// 请求方法名
        /// </summary>
        public string RequestMethodName { get; set; } = string.Empty;
        /// <summary>
        /// 请求参数名
        /// </summary>
        public string RequestParamsName { get; set; } = string.Empty;
        /// <summary>
        /// 请求参数数据JSON
        /// </summary>
        public string RequestParamsData { get; set; } = string.Empty;
        /// <summary>
        /// 请求响应间隔时间
        /// </summary>
        public string ResponseIntervalTime { get; set; } = string.Empty;
        /// <summary>
        /// 响应时间
        /// </summary>
        public string ResponseTime { get; set; } = string.Empty;
        /// <summary>
        /// 响应结果
        /// </summary>
        public string ResponseJsonData { get; set; } = string.Empty;
    }

    public class ApiLogAopExInfo
    {
        public ApiLogAopInfo ApiLogAopInfo { get; set; }
        /// <summary>
        /// 异常
        /// </summary>
        public string InnerException { get; set; } = string.Empty;
        /// <summary>
        /// 异常信息
        /// </summary>
        public string ExMessage { get; set; } = string.Empty;
    }
}
