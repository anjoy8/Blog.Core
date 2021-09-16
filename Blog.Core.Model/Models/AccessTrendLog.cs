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
        /// 
        /// </summary>
        [SugarColumn(Length = 128, IsNullable = true, ColumnDataType = "nvarchar")]
        public string User { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(Length = 128, IsNullable = true, ColumnDataType = "nvarchar")]
        public string IP { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(Length = 128, IsNullable = true, ColumnDataType = "nvarchar")]
        public string API { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(Length = 128, IsNullable = true, ColumnDataType = "nvarchar")]
        public string BeginTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(Length = 128, IsNullable = true, ColumnDataType = "nvarchar")]
        public string OPTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(Length = 128, IsNullable = true, ColumnDataType = "nvarchar")]
        public string RequestMethod { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(Length = 256, IsNullable = true, ColumnDataType = "nvarchar")]
        public string RequestData { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(Length = 256, IsNullable = true, ColumnDataType = "nvarchar")]
        public string Agent { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Createdate { get; set; } = DateTime.Now;
    }
}
