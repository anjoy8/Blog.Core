namespace Blog.Core.Migrate.Attributes;

/// <summary>
/// 数据字典 <br />
/// 程序内置Code和Name
/// 例如 <br />
/// SO:销售订单<br />
/// 1:男
/// </summary>
[AttributeUsage(AttributeTargets.Enum)]
public class DataDictionaryAttribute : Attribute
{
}