using Blog.Core.Common.Option.Core;

namespace Blog.Core.Common.Option;

public class SeqOptions : IConfigurableOptions
{
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// 地址
    /// </summary>
    public string Address { get; set; }

    public string ApiKey { get; set; }
}