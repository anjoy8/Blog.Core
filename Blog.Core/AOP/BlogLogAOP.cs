using Blog.Core.Common.LogHelper;
using Blog.Core.Hubs;
using Blog.Core.Model.Models;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.SignalR;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Core.AOP
{
    /// <summary>
    /// 拦截器BlogLogAOP 继承IInterceptor接口
    /// </summary>
    public class BlogLogAOP : IInterceptor
    {
        private readonly IHubContext<ChatHub> _hubContext;
        public BlogLogAOP(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;

        }


        /// <summary>
        /// 实例化IInterceptor唯一方法 
        /// </summary>
        /// <param name="invocation">包含被拦截方法的信息</param>
        public void Intercept(IInvocation invocation)
        {
            //记录被拦截方法信息的日志信息
            var dataIntercept = "" +
                $"【当前执行方法】：{ invocation.Method.Name} \r\n" +
                $"【携带的参数有】： {string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray())} \r\n";

            try
            {
                MiniProfiler.Current.Step($"执行Service方法：{invocation.Method.Name}() -> ");
                //在被拦截的方法执行完毕后 继续执行当前方法，注意是被拦截的是异步的
                invocation.Proceed();


                // 异步获取异常，先执行
                if (IsAsyncMethod(invocation.Method))
                {

                    //Wait task execution and modify return value
                    if (invocation.Method.ReturnType == typeof(Task))
                    {
                        invocation.ReturnValue = InternalAsyncHelper.AwaitTaskWithPostActionAndFinally(
                            (Task)invocation.ReturnValue,
                            async () => await TestActionAsync(invocation),
                            ex =>
                            {
                                LogEx(ex, ref dataIntercept);
                            });
                    }
                    else //Task<TResult>
                    {
                        invocation.ReturnValue = InternalAsyncHelper.CallAwaitTaskWithPostActionAndFinallyAndGetResult(
                         invocation.Method.ReturnType.GenericTypeArguments[0],
                         invocation.ReturnValue,
                         async () => await TestActionAsync(invocation),
                         ex =>
                         {
                             LogEx(ex, ref dataIntercept);
                         });

                    }

                }
                else
                {// 同步1


                }
            }
            catch (Exception ex)// 同步2
            {
                LogEx(ex, ref dataIntercept);

            }

            dataIntercept += ($"【执行完成结果】：{invocation.ReturnValue}");

            Parallel.For(0, 1, e =>
            {
                LogLock.OutSql2Log("AOPLog", new string[] { dataIntercept });
            });

            _hubContext.Clients.All.SendAsync("ReceiveUpdate", LogLock.GetLogData()).Wait();


        }

        private async Task TestActionAsync(IInvocation invocation)
        {
            //Console.WriteLine("Waiting after method execution for " + invocation.MethodInvocationTarget.Name);
            await Task.Delay(20); // 仅作测试
            //Console.WriteLine("Waited after method execution for " + invocation.MethodInvocationTarget.Name);
        }

        private void LogEx(Exception ex, ref string dataIntercept)
        {
            if (ex != null)
            {
                //执行的 service 中，收录异常
                MiniProfiler.Current.CustomTiming("Errors：", ex.Message);
                //执行的 service 中，捕获异常
                dataIntercept += ($"方法执行中出现异常：{ex.Message + ex.InnerException}\r\n");
            }
        }


        public static bool IsAsyncMethod(MethodInfo method)
        {
            return (
                method.ReturnType == typeof(Task) ||
                (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
                );
        }

    }


    internal static class InternalAsyncHelper
    {
        public static async Task AwaitTaskWithPostActionAndFinally(Task actualReturnValue, Func<Task> postAction, Action<Exception> finalAction)
        {
            Exception exception = null;

            try
            {
                await actualReturnValue;
                await postAction();
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                finalAction(exception);
            }
        }

        public static async Task<T> AwaitTaskWithPostActionAndFinallyAndGetResult<T>(Task<T> actualReturnValue, Func<Task> postAction, Action<Exception> finalAction)
        {
            Exception exception = null;

            try
            {
                var result = await actualReturnValue;
                await postAction();
                return result;
            }
            catch (Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {
                finalAction(exception);
            }
        }

        public static object CallAwaitTaskWithPostActionAndFinallyAndGetResult(Type taskReturnType, object actualReturnValue, Func<Task> action, Action<Exception> finalAction)
        {
            return typeof(InternalAsyncHelper)
                .GetMethod("AwaitTaskWithPostActionAndFinallyAndGetResult", BindingFlags.Public | BindingFlags.Static)
                .MakeGenericMethod(taskReturnType)
                .Invoke(null, new object[] { actualReturnValue, action, finalAction });
        }
    }

}
