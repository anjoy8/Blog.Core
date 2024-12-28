namespace Blog.Core.Migrate.Attributes;

/// <summary>
/// 迁移
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class MigrateAttribute : Attribute
{
    /// <summary>
    /// 是否开启迁移
    /// </summary>
    public bool Enable { get; set; } = true;
}