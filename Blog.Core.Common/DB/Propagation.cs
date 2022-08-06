namespace Blog.Core.Common.DB;

public enum Propagation
{
    /// <summary>
    /// 默认：如果当前没有事务，就新建一个事务，如果已存在一个事务中，加入到这个事务中。
    /// </summary>
    Required = 0,
    
    /// <summary>
    /// 使用当前事务，如果没有当前事务，就抛出异常
    /// </summary>
    Mandatory = 1,

    /// <summary>
    /// 以嵌套事务方式执行
    /// </summary>
    Nested = 2,
}