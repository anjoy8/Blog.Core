using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Blog.Core.Model.ViewModels
{
    /// <summary>
    /// 微信验证Dto
    /// 作者:胡丁文
    /// 时间:2020-4-1 21:34:07
    /// </summary> 
    public class WeChatValidDto
    {
        /// <summary>
        /// 微信公众号唯一标识
        /// </summary>
        public string publicAccount { get; set; }
        /// <summary>
        /// 验证成功后返回给微信的字符串
        /// </summary>
        public string echoStr { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string signature { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public string timestamp { get; set; }
        /// <summary>
        /// 随机数
        /// </summary>
        public string nonce { get; set; } 

    }
}
