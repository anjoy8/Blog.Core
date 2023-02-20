using System;

namespace Blog.Core.Model.Tenants;

/// <summary>
/// 标识 多租户-分库 的业务表 <br/>
/// 公共表无需区分 直接使用主库 各自业务在各自库中
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class MultiTenantAttribute : Attribute
{
}