using Blog.Core.Model.Base.Tenants;

namespace Blog.Core.Model.Models;

/// <summary>
/// 系统租户表 <br/>
/// 根据TenantType 分为两种方案: <br/>
/// 1.按租户字段区分<br/>
/// 2.按租户分库<br/>
/// 
/// <br/>
/// 
/// 注意:<br/>
/// 使用租户Id方案,无需配置分库的连接
/// </summary>
[MigrateVersion("1.0.0")]
public class SysTenant : RootEntityTkey<long>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 租户类型
    /// </summary>
    public TenantTypeEnum TenantType { get; set; }

    /// <summary>
    /// 数据库/租户标识 不可重复<br/>
    /// 使用Id方案,可无需配置
    /// </summary>
    [SugarColumn(Length = 64)]
    public string ConfigId { get; set; }

    /// <summary>
    /// 主机<br/>
    /// 使用Id方案,可无需配置
    /// </summary>
    [SugarColumn(IsNullable = true)]
    public string Host { get; set; }

    /// <summary>
    /// 数据库类型<br/>
    /// 使用Id方案,可无需配置
    /// </summary>
    [SugarColumn(IsNullable = true)]
    public SqlSugar.DbType? DbType { get; set; }

    /// <summary>
    /// 数据库连接<br/>
    /// 使用Id方案,可无需配置
    /// </summary>
    [SugarColumn(IsNullable = true)]
    public string Connection { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public bool Status { get; set; } = true;

    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(IsNullable = true)]
    public string Remark { get; set; }
}