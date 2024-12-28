using Blog.Core.Model.Base.Tenants;

namespace Blog.Core.Model.Models.Tenant;

/// <summary>
/// 多租户-多表方案 业务表 <br/>
/// </summary>
[MultiTenant(TenantTypeEnum.Tables)]
[MigrateVersion("1.0.1")]
public class MultiBusinessTable : BaseEntity
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
    /// 备注(测试增加字段,多表迁移)
    /// </summary>
    [SugarColumn(IsNullable = true)]
    public string Remark { get; set; }

    [Navigate(NavigateType.OneToMany, nameof(MultiBusinessSubTable.MainId))]
    public List<MultiBusinessSubTable> Child { get; set; }
}