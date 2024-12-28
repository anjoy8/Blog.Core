using Blog.Core.Model.Base.Tenants;

namespace Blog.Core.Model.Models.Tenant;

/// <summary>
/// 多租户-多库方案 业务表 <br/>
/// 公共库无需标记[MultiTenant]特性
/// </summary>
[MultiTenant]
[MigrateVersion("1.0.1")]
public class SubLibraryBusinessTable : BaseEntity
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// 备注(测试增加字段,多库迁移)
    /// </summary>
    [SugarColumn(IsNullable = true)]
    public string Remark { get; set; }
}