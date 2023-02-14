using System.ComponentModel;

namespace Blog.Core.Model.CustomEnums;

/// <summary>
/// 租户隔离方案
/// </summary>
public enum TenantTypeEnum
{
    None = 0,

    /// <summary>
    /// Id隔离
    /// </summary>
    [Description("Id隔离")]
    Id = 1,

    /// <summary>
    /// 库隔离
    /// </summary>
    [Description("库隔离")]
    Db = 2,
}