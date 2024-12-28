namespace Blog.Core.Migrate.Attributes;

/// <summary>
/// 标记版本 <br />
/// 提高CodeFirst的性能 不必要做性能牺牲 <br />
/// 修改实体的字段、特性、索引等 <br />
/// 一定要修改版本号！！！ <br />
/// 一定要修改版本号！！！ <br />
/// 一定要修改版本号！！！ <br />
/// 否则不会主动修改
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class MigrateVersionAttribute : Attribute
{
    public MigrateVersionAttribute(string version)
    {
        Version = Version.Parse(version);
    }

    public Version Version { get; set; }
}