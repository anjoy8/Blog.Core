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
}
