using SqlSugar;
using System;

namespace Blog.Core.Model.Models
{
    public class Guestbook
    {

        /// <summary>
        /// 留言表
        /// </summary>
        [SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = true)]
        public int id { get; set; }

        /// <summary>博客ID
        /// 
        /// </summary>
        public int? blogId { get; set; }
        /// <summary>创建时间
        /// 
        /// </summary>
        public DateTime createdate { get; set; }

        [SugarColumn(Length = int.MaxValue, IsNullable = true,ColumnDataType ="nvarchar")]
        public string username { get; set; }

        /// <summary>手机
        /// 
        /// </summary>
        [SugarColumn(Length = int.MaxValue, IsNullable = true,ColumnDataType ="nvarchar")]
        public string phone { get; set; }
        /// <summary>qq
        /// 
        /// </summary>
        [SugarColumn(Length = int.MaxValue, IsNullable = true,ColumnDataType ="nvarchar")]
        public string QQ { get; set; }

        /// <summary>留言内容
        /// 
        /// </summary>
        [SugarColumn(Length = int.MaxValue, IsNullable = true,ColumnDataType ="nvarchar")]
        public string body { get; set; }
        /// <summary>ip地址
        /// 
        /// </summary>
        [SugarColumn(Length = int.MaxValue, IsNullable = true,ColumnDataType ="nvarchar")]
        public string ip { get; set; }

        /// <summary>是否显示在前台,0否1是
        /// 
        /// </summary>
        public bool isshow { get; set; }

        [SugarColumn(IsIgnore = true)]
        public BlogArticle blogarticle { get; set; }
    }
}
