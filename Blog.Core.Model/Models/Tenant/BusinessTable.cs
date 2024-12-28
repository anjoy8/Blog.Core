using Blog.Core.Model.Base.Tenants;

namespace Blog.Core.Model.Models.Tenant;

/// <summary>
/// 业务数据 <br/>
/// 多租户 (Id 隔离)
/// </summary>
[MigrateVersion("1.0.0")]
public class BusinessTable : BaseEntity, ITenantEntity
{
    /// <summary>
    /// 无需手动赋值
    /// </summary>
    public long TenantId { get; set; }


    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    public decimal Amount { get; set; }
}