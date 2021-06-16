namespace Blog.Core.Common.Helper
{
    public  class UrlHelper
    {
        /// <summary>
        /// UrlEncode编码
        /// </summary>
        /// <param name="url">url</param>
        /// <returns></returns>
        public static string UrlEncode(string url) {
            return System.Web.HttpUtility.UrlEncode(url, System.Text.Encoding.UTF8); 
        }
        /// <summary>
        ///  UrlEncode解码
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public static string UrlDecode(string data)
        {
            return System.Web.HttpUtility.UrlDecode(data, System.Text.Encoding.UTF8);
        }
    }
}
