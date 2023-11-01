using Blog.Core.Common.Option.Core;

namespace Blog.Core.Common.Option;

/// <summary>
/// 缓存配置选项
/// </summary>
public sealed class RedisOptions : IConfigurableOptions
{
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enable { get; set; }

    /// <summary>
    /// Redis连接
    /// </summary>
    public string ConnectionString { get; set; }

    /// <summary>
    /// 键值前缀
    /// </summary>
    public string InstanceName { get; set; }
}