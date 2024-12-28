namespace Blog.Core.Model.ViewModels
{
    /// <summary>
    /// 返回给调用者的Dto
    /// 作者:胡丁文
    /// 时间:2020-4-8 09:52:06
    /// </summary>
    public class WeChatResponseUserInfo
    {
        /// <summary>
        /// 微信公众号ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 公司代码
        /// </summary>
        public string companyCode { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public WeChatApiDto usersData { get; set; }
    }
}
