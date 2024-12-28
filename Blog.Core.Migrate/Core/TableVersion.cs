using SqlSugar;

namespace Blog.Core.Migrate.Core;

/// <summary>
/// 数据表版本
/// </summary>
public class TableVersion
{
    /// <summary>
    /// 表名称
    /// </summary>
    [SugarColumn(Length = 128, IsPrimaryKey = true)]
    public string TableName { get; set; }

    [SugarColumn(Length = 50)]
    public string Version { get; set; }
}