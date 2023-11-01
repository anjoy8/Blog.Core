using SqlSugar;
using System;

namespace Blog.Core.Model.Models
{
    /// <summary>
    /// 用户团队表
    /// </summary>
    [SugarTable("GblLogAudit", TableDescription = "日志审计")]
    public class GblLogAudit
    {
        ///<summary>
        ///ID
        ///</summary>
        [SugarColumn(ColumnDescription = "ID", IsNullable = false, IsPrimaryKey = true, IsIdentity = false)]
        public long Id { get; set; }

        ///<summary>
        ///HttpContext.TraceIdentifier 事件链路ID（获取或设置一个唯一标识符，用于在跟踪日志中表示此请求。）
        ///</summary>
        [SugarColumn(ColumnDescription = "事件链路ID", IsNullable = false, IsPrimaryKey = false, IsIdentity = false, Length = 255)]
        public string TraceId { get; set; }

        ///<summary>
        ///时间
        ///</summary>
        [SugarColumn(ColumnDescription = "时间", IsNullable = false, IsPrimaryKey = false, IsIdentity = false)]
        public DateTime Date { get; set; } = DateTime.Now;

        ///<summary>
        ///线程
        ///</summary>
        [SugarColumn(ColumnDescription = "线程", IsNullable = false, IsPrimaryKey = false, IsIdentity = false, Length = 255)]
        public string Thread { get; set; }

        ///<summary>
        ///等级
        ///</summary>
        [SugarColumn(ColumnDescription = "等级", IsNullable = false, IsPrimaryKey = false, IsIdentity = false, Length = 255)]
        public string Level { get; set; }
        ///<summary>
        ///记录器
        ///</summary>
        [SugarColumn(ColumnDescription = "记录器", IsNullable = false, IsPrimaryKey = false, IsIdentity = false, Length = 255)]
        public string Logger { get; set; }
        ///<summary>
        ///日志类型
        ///</summary>
        [SugarColumn(ColumnDescription = "日志类型", IsNullable = false, IsPrimaryKey = false, IsIdentity = false, Length = 255)]
        public string LogType { get; set; }
        ///<summary>
        ///数据类型
        ///</summary>
        [SugarColumn(ColumnDescription = "数据类型", IsNullable = false, IsPrimaryKey = false, IsIdentity = false, Length = 255)]
        public string DataType { get; set; }

        ///<summary>
        ///错误信息
        ///</summary>
        [SugarColumn(ColumnDescription = "错误信息", IsNullable = false, IsPrimaryKey = false, IsIdentity = false, Length = 2000)]
        public string Message { get; set; }

        ///<summary>
        ///异常
        ///</summary>
        [SugarColumn(ColumnDescription = "异常", IsNullable = true, IsPrimaryKey = false, IsIdentity = false, Length = 2000)]
        public string Exception { get; set; }

    }
}