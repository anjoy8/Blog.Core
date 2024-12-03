using Blog.Core.Common.Option.Core;

namespace Blog.Core.Common.Option;

public class AppSettingsOption : IConfigurableOptions
{
    /// <summary>
    /// 迁移表结构
    /// </summary>
    public bool MigrateDBEnabled { get; set; }

    /// <summary>
    /// 初始化数据
    /// </summary>
    public bool SeedDBEnabled { get; set; }

    /// <summary>
    /// 生成测试数据
    /// </summary>
    public bool TestSeedDbEnabled { get; set; }

    public string Author { get; set; }

    /// <summary>
    /// 是否启用数据库二级缓存
    /// </summary>
    public bool CacheDbEnabled { get; set; } = false;
}