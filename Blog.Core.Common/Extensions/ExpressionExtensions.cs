using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog.Core.Common.Helper
{
    /// <summary>
    /// Linq扩展
    /// </summary>
    public static class ExpressionExtensions
    {
        #region HttpContext

        /// <summary>
        /// 返回请求上下文
        /// </summary>
        /// <param name="context"></param>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="ContentType"></param>
        /// <returns></returns>
        public static async Task Cof_SendResponse(this HttpContext context, System.Net.HttpStatusCode code, string message, string ContentType = "text/html;charset=utf-8")
        {
            context.Response.StatusCode = (int)code;
            context.Response.ContentType = ContentType;
            await context.Response.WriteAsync(message);
        }

        #endregion

        #region ICaching

        /// <summary>
        /// 从缓存里取数据，如果不存在则执行查询方法，
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="cache">ICaching </param>
        /// <param name="key">键值</param>
        /// <param name="GetFun">查询方法</param>
        /// <param name="timeSpanMin">有效期 单位分钟/param>
        /// <returns></returns>
        public static T Cof_GetICaching<T>(this ICaching cache, string key, Func<T> GetFun, int timeSpanMin) where T : class
        {
            var obj = cache.Get(key);
            obj = GetFun();
            if (obj == null)
            {
                obj = GetFun();
                cache.Set(key, obj, timeSpanMin);
            }

            return obj as T;
        }

        /// <summary>
        /// 异步从缓存里取数据，如果不存在则执行查询方法
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="cache">ICaching </param>
        /// <param name="key">键值</param>
        /// <param name="GetFun">查询方法</param>
        /// <param name="timeSpanMin">有效期 单位分钟/param>
        /// <returns></returns>
        public static async Task<T> Cof_AsyncGetICaching<T>(this ICaching cache, string key, Func<Task<T>> GetFun, int timeSpanMin) where T : class
        {
            var obj = cache.Get(key);
            if (obj == null)
            {
                obj = await GetFun();
                cache.Set(key, obj, timeSpanMin);
            }

            return obj as T;
        }

        #endregion

        #region 常用扩展方法

        public static bool Cof_CheckAvailable<TSource>(this IEnumerable<TSource> Tlist)
        {
            return Tlist != null && Tlist.Count() > 0;
        }

        /// <summary>
        /// 调用内部方法
        /// </summary>
        public static Expression Call(this Expression instance, string methodName, params Expression[] arguments)
        {
            if (instance.Type == typeof(string))
                return Expression.Call(instance, instance.Type.GetMethod(methodName, new Type[] { typeof(string) }), arguments); //修复string contains 出现的问题 Ambiguous match found.
            else
                return Expression.Call(instance, instance.Type.GetMethod(methodName), arguments);
        }

        /// <summary>
        /// 获取内部成员
        /// </summary>
        public static Expression Property(this Expression expression, string propertyName)
        {
            // Todo:左边条件如果是dynamic，
            // 则Expression.Property无法获取子内容
            // 报错在这里，由于expression内的对象为Object，所以无法解析到
            // var x = (expression as IQueryable).ElementType;
            var exp = Expression.Property(expression, propertyName);
            if (exp.Type.IsGenericType && exp.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return Expression.Convert(exp, exp.Type.GetGenericArguments()[0]);
            }

            return exp;
        }

        /// <summary>
        /// 转Lambda
        /// </summary>
        public static Expression<TDelegate> ToLambda<TDelegate>(this Expression body,
            params ParameterExpression[] parameters)
        {
            return Expression.Lambda<TDelegate>(body, parameters);
        }

        #endregion

        #region 常用运算符 [ > , >= , == , < , <= , != , || , && ]

        /// <summary>
        /// &&
        /// </summary>
        public static Expression AndAlso(this Expression left, Expression right)
        {
            return Expression.AndAlso(left, right);
        }

        /// <summary>
        /// ||
        /// </summary>
        public static Expression OrElse(this Expression left, Expression right)
        {
            return Expression.OrElse(left, right);
        }

        /// <summary>
        /// Contains
        /// </summary>
        public static Expression Contains(this Expression left, Expression right)
        {
            return left.Call("Contains", right);
        }

        public static Expression StartContains(this Expression left, Expression right)
        {
            return left.Call("StartsWith", right);
        }

        public static Expression EndContains(this Expression left, Expression right)
        {
            return left.Call("EndsWith", right);
        }

        /// <summary>
        /// >
        /// </summary>
        public static Expression GreaterThan(this Expression left, Expression right)
        {
            return Expression.GreaterThan(left, right);
        }

        /// <summary>
        /// >=
        /// </summary>
        public static Expression GreaterThanOrEqual(this Expression left, Expression right)
        {
            return Expression.GreaterThanOrEqual(left, right);
        }

        /// <summary>
        /// <
        /// </summary>
        public static Expression LessThan(this Expression left, Expression right)
        {
            return Expression.LessThan(left, right);
        }

        /// <summary>
        /// <=
        /// </summary>
        public static Expression LessThanOrEqual(this Expression left, Expression right)
        {
            return Expression.LessThanOrEqual(left, right);
        }

        /// <summary>
        /// ==
        /// </summary>
        public static Expression Equal(this Expression left, Expression right)
        {
            return Expression.Equal(left, right);
        }

        /// <summary>
        /// !=
        /// </summary>
        public static Expression NotEqual(this Expression left, Expression right)
        {
            return Expression.NotEqual(left, right);
        }

        #endregion
    }
}