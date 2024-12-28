namespace Blog.Core.Model.Base.Tenants;

/// <summary>
/// 租户模型接口
/// </summary>
public interface ITenantEntity
{
    /// <summary>
    /// 租户Id
    /// </summary>
    [SugarColumn(DefaultValue = "0")]
    public long TenantId { get; set; }
}