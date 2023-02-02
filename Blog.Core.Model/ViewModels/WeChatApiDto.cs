using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Core.Model.ViewModels
{
    /// <summary>
    /// 微信接口消息DTO
    /// 作者:胡丁文
    /// 时间:2020-03-25
    /// </summary>
    public class WeChatApiDto
    {
        /// <summary>
        /// 微信公众号ID(数据库查询)
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 错误代码
        /// </summary>
        public int errcode { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string errmsg { get; set; }


        /// <summary>
        /// token
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// 过期时间(秒)
        /// </summary>
        public int expires_in { get; set; } 


        /// <summary>
        /// 用户关注数
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// 获取用户数量
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 获取用户OpenIDs
        /// </summary>
        public WeChatOpenIDsDto data { get; set; }
        public List<WeChatApiDto> users { get; set; }
        /// <summary>
        /// 下一个关注用户
        /// </summary>
        public string next_openid { get; set; }

        /// <summary>
        /// 微信消息模板列表
        /// </summary>

        public WeChatTemplateList[] template_list { get; set; }
        /// <summary>
        /// 微信菜单
        /// </summary>
        public WeChatMenuDto menu { get; set; }

        /// <summary>
        /// 二维码票据
        /// </summary>
        public string ticket { get; set; }
        /// <summary>
        /// 二维码过期时间
        /// </summary>
        public int expire_seconds { get; set; }
        /// <summary>
        /// 二维码地址
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 关注状态
        /// </summary> 
        public string subscribe { get; set; }
        /// <summary>
        /// 用户微信ID
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string nickname { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int sex { get; set; }
        /// <summary>
        /// 语言
        /// </summary>
        public string language { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string country { get; set; }
        /// <summary>
        /// 头像地址
        /// </summary>
        public string headimgurl { get; set; } 

    }
}
