using Blog.Core.Model.Base.Tenants;

namespace Blog.Core.Model.Models.Tenant;

/// <summary>
/// 多租户-多表方案 业务表 子表 <br/>
/// </summary>
[MultiTenant(TenantTypeEnum.Tables)]
[MigrateVersion("1.0.0")]
public class MultiBusinessSubTable : BaseEntity
{
    public long MainId { get; set; }
    public string Memo { get; set; }
}