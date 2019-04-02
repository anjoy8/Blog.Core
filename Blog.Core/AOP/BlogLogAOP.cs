using Castle.DynamicProxy;
using StackExchange.Profiling;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Core.AOP
{
    /// <summary>
    /// 拦截器BlogLogAOP 继承IInterceptor接口
    /// </summary>
    public class BlogLogAOP : IInterceptor
    {

        static ReaderWriterLockSlim LogWriteLock = new ReaderWriterLockSlim();
        static int LogCount = 100;
        static int WritedCount = 0;
        static int FailedCount = 0;

        /// <summary>
        /// 实例化IInterceptor唯一方法 
        /// </summary>
        /// <param name="invocation">包含被拦截方法的信息</param>
        public void Intercept(IInvocation invocation)
        {
            //记录被拦截方法信息的日志信息
            var dataIntercept = $"{DateTime.Now:yyyyMMdd HH:mm:ss} " +
                $"当前执行方法：{ invocation.Method.Name} " +
                $"参数是： {string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray())} \r\n";

            try
            {
                MiniProfiler.Current.Step($"执行Service方法：{invocation.Method.Name}() -> ");
                //在被拦截的方法执行完毕后 继续执行当前方法，注意是被拦截的是异步的
                invocation.Proceed();
            }
            catch (Exception e)
            {
                //执行的 service 中，收录异常
                MiniProfiler.Current.CustomTiming("Errors：", e.Message);
                //执行的 service 中，捕获异常
                dataIntercept += ($"方法执行中出现异常：{e.Message + e.InnerException}");
            }

            dataIntercept += ($"方法执行完毕，返回结果：{invocation.ReturnValue}");

            Parallel.For(0, 1, e =>
            {
                OutSql2Log(dataIntercept);
            });

            #region //输出到当前项目日志,多线程可能会出现争抢资源的问题，舍弃//
            //var path = Directory.GetCurrentDirectory() + @"\Log";
            //if (!Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);
            //}

            //string fileName = path + $@"\AOPLog.log";

            //StreamWriter sw = File.AppendText(fileName);
            //sw.WriteLine(dataIntercept);
            //sw.WriteLine();
            //sw.Close();
            #endregion

        }

        static void OutSql2Log(string dataIntercept)
        {
            try
            {
                //设置读写锁为写入模式独占资源，其他写入请求需要等待本次写入结束之后才能继续写入
                //注意：长时间持有读线程锁或写线程锁会使其他线程发生饥饿 (starve)。 为了得到最好的性能，需要考虑重新构造应用程序以将写访问的持续时间减少到最小。
                //      从性能方面考虑，请求进入写入模式应该紧跟文件操作之前，在此处进入写入模式仅是为了降低代码复杂度
                //      因进入与退出写入模式应在同一个try finally语句块内，所以在请求进入写入模式之前不能触发异常，否则释放次数大于请求次数将会触发异常
                LogWriteLock.EnterWriteLock();

                var path = Directory.GetCurrentDirectory() + @"\Log";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string logFilePath = path + $@"\AOPLog.log";

                var now = DateTime.Now;
                var logContent = string.Format(
                    "--------------------------------\n" +
                    DateTime.Now + "\n" +
                    dataIntercept + "\n" +
                    "--------------------------------\n"
                    );

                File.AppendAllText(logFilePath, logContent);
                WritedCount++;
            }
            catch (Exception)
            {
                FailedCount++;
            }
            finally
            {
                //退出写入模式，释放资源占用
                //注意：一次请求对应一次释放
                //      若释放次数大于请求次数将会触发异常[写入锁定未经保持即被释放]
                //      若请求处理完成后未释放将会触发异常[此模式不下允许以递归方式获取写入锁定]
                LogWriteLock.ExitWriteLock();
            }
        }

    }
}
