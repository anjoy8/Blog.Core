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
        [SugarColumn(ColumnDescription = "ID", IsNullable = false, IsPrimaryKey = false, IsIdentity = true)]
        public int Id { get; set; }

        ///<summary>
        ///时间
        ///</summary>
        [SugarColumn(ColumnDescription = "时间", IsNullable = false, IsPrimaryKey = false, IsIdentity = false, ColumnDataType = "datetime")]
        public DateTime Date { get; set; } = DateTime.Now;

        ///<summary>
        ///线程
        ///</summary>
        [SugarColumn(ColumnDescription = "线程", IsNullable = false, IsPrimaryKey = false, IsIdentity = false, ColumnDataType = "varchar", Length = 255)]
        public string Thread { get; set; }

        ///<summary>
        ///等级
        ///</summary>
        [SugarColumn(ColumnDescription = "等级", IsNullable = false, IsPrimaryKey = false, IsIdentity = false, ColumnDataType = "varchar", Length = 255)]
        public string Level { get; set; }
        ///<summary>
        ///记录器
        ///</summary>
        [SugarColumn(ColumnDescription = "记录器", IsNullable = false, IsPrimaryKey = false, IsIdentity = false, ColumnDataType = "varchar", Length = 255)]
        public string Logger { get; set; }
        ///<summary>
        ///日志类型
        ///</summary>
        [SugarColumn(ColumnDescription = "日志类型", IsNullable = false, IsPrimaryKey = false, IsIdentity = false, ColumnDataType = "varchar", Length = 255)]
        public string LogType { get; set; }
        ///<summary>
        ///数据类型
        ///</summary>
        [SugarColumn(ColumnDescription = "数据类型", IsNullable = false, IsPrimaryKey = false, IsIdentity = false, ColumnDataType = "varchar", Length = 255)]
        public string DataType { get; set; }

        ///<summary>
        ///错误信息
        ///</summary>
        [SugarColumn(ColumnDescription = "错误信息", IsNullable = false, IsPrimaryKey = false, IsIdentity = false, ColumnDataType = "text")]
        public string Message { get; set; }

        ///<summary>
        ///异常
        ///</summary>
        [SugarColumn(ColumnDescription = "异常", IsNullable = true, IsPrimaryKey = false, IsIdentity = false, ColumnDataType = "text")]
        public string Exception { get; set; }

    }
}