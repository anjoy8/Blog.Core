namespace Blog.Core.Model.Base.Tenants;

/// <summary>
/// 标识 多租户 的业务表 <br/>
/// 默认设置是多库       <br/>
/// 公共表无需区分 直接使用主库 各自业务在各自库中 <br/>
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class MultiTenantAttribute : Attribute
{
    public MultiTenantAttribute()
    {
    }

    public MultiTenantAttribute(TenantTypeEnum tenantType)
    {
        TenantType = tenantType;
    }


    public TenantTypeEnum TenantType { get; set; } = TenantTypeEnum.Db;
}