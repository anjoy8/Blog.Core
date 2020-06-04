using System;
using Blog.Core.Model.Models;

namespace Blog.Core.Model.ViewModels
{
    /// <summary>
    /// 留言信息展示类
    /// </summary>
    public class GuestbookViewModels
    {
        /// <summary>留言表
        /// 
        /// </summary>
        public int id { get; set; }

        /// <summary>博客ID
        /// 
        /// </summary>
        public int? blogId { get; set; }
        /// <summary>创建时间
        /// 
        /// </summary>
        public DateTime createdate { get; set; }
        public string username { get; set; }

        /// <summary>手机
        /// 
        /// </summary>
        public string phone { get; set; }
        /// <summary>qq
        /// 
        /// </summary>
        public string QQ { get; set; }

        /// <summary>留言内容
        /// 
        /// </summary>
        public string body { get; set; }
        /// <summary>ip地址
        /// 
        /// </summary>
        public string ip { get; set; }

        /// <summary>是否显示在前台,0否1是
        /// 
        /// </summary>
        public bool isshow { get; set; }

        public BlogArticle blogarticle { get; set; }
    }
}
