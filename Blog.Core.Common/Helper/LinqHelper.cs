using System;
using System.Linq.Expressions;

namespace Blog.Core.Common.Helper
{
    /// <summary>
    /// Linq操作帮助类
    /// </summary>
    public static class LinqHelper
    {
        /// <summary>
        /// 创建初始条件为True的表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> True<T>()
        {
            return x => true;
        }

        /// <summary>
        /// 创建初始条件为False的表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> False<T>()
        {
            return x => false;
        }
    }
}
