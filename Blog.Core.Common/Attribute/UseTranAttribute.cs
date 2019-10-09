using System;

namespace Blog.Core.Common
{
    /// <summary>
    /// 这个Attribute就是使用时候的验证，把它添加到需要执行事务的方法中，即可完成事务的操作。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class UseTranAttribute : Attribute
    {

    }
}
