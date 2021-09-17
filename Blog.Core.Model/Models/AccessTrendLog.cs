using SqlSugar;
using System;

namespace Blog.Core.Model.Models
{
    /// <summary>
    /// 用户访问趋势日志
    /// </summary>
    public class AccessTrendLog : RootEntityTkey<int>
    {
        /// <summary>
        /// 用户
        /// </summary>
        [SugarColumn(Length = 128, IsNullable = true, ColumnDataType = "nvarchar")]
        public string User { get; set; }

        /// <summary>
        /// 次数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; } = DateTime.Now;
    }
}
