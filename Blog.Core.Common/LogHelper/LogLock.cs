using Blog.Core.Common.Helper;
using Blog.Core.Hubs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Blog.Core.Common.LogHelper
{
    public class LogLock
    {

        static ReaderWriterLockSlim LogWriteLock = new ReaderWriterLockSlim();
        static int WritedCount = 0;
        static int FailedCount = 0;
        static string _contentRoot = string.Empty;

        public LogLock(string contentPath)
        {
            _contentRoot = contentPath;
        }

        public static void OutSql2Log(string filename, string[] dataParas)
        {
            try
            {
                //设置读写锁为写入模式独占资源，其他写入请求需要等待本次写入结束之后才能继续写入
                //注意：长时间持有读线程锁或写线程锁会使其他线程发生饥饿 (starve)。 为了得到最好的性能，需要考虑重新构造应用程序以将写访问的持续时间减少到最小。
                //      从性能方面考虑，请求进入写入模式应该紧跟文件操作之前，在此处进入写入模式仅是为了降低代码复杂度
                //      因进入与退出写入模式应在同一个try finally语句块内，所以在请求进入写入模式之前不能触发异常，否则释放次数大于请求次数将会触发异常
                LogWriteLock.EnterWriteLock();

                var path = Path.Combine(_contentRoot, "Log");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string logFilePath = Path.Combine(path, $@"{filename}.log");

                var now = DateTime.Now;
                var logContent = (
                    "--------------------------------\r\n" +
                    DateTime.Now + "|\r\n" +
                    String.Join("\r\n", dataParas) + "\r\n"
                    );

                File.AppendAllText(logFilePath, logContent);
                WritedCount++;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
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

        public static string ReadLog(string Path, Encoding encode)
        {
            string s = "";
            try
            {
                LogWriteLock.EnterReadLock();

                if (!System.IO.File.Exists(Path))
                {
                    s = null;
                }
                else
                {
                    StreamReader f2 = new StreamReader(Path, encode);
                    s = f2.ReadToEnd();
                    f2.Close();
                    f2.Dispose();
                }
            }
            catch (Exception)
            {
                FailedCount++;
            }
            finally
            {
                LogWriteLock.ExitReadLock();
            }
            return s;
        }


        public static List<LogInfo> GetLogData()
        {
            List<LogInfo> aopLogs = new List<LogInfo>();
            List<LogInfo> excLogs = new List<LogInfo>();
            List<LogInfo> sqlLogs = new List<LogInfo>();
            List<LogInfo> reqresLogs = new List<LogInfo>();
            try
            {
                aopLogs = ReadLog(Path.Combine(_contentRoot, "Log", "AOPLog.log"), Encoding.UTF8)
                .Split("--------------------------------")
                .Where(d => !string.IsNullOrEmpty(d) && d != "\n" && d != "\r\n")
                .Select(d => new LogInfo
                {
                    Datetime = d.Split("|")[0].ObjToDate(),
                    Content = d.Split("|")[1]?.Replace("\r\n", "<br>"),
                    LogColor = "AOP",
                }).ToList();

            }
            catch (Exception)
            {
            }

            try
            {
                excLogs = ReadLog(Path.Combine(_contentRoot, "Log", $"GlobalExcepLogs_{DateTime.Now.ToString("yyyMMdd")}.log"), Encoding.UTF8)?
                      .Split("--------------------------------")
                      .Where(d => !string.IsNullOrEmpty(d) && d != "\n" && d != "\r\n")
                      .Select(d => new LogInfo
                      {
                          Datetime = (d.Split("|")[0]).Split(',')[0].ObjToDate(),
                          Content = d.Split("|")[1]?.Replace("\r\n", "<br>"),
                          LogColor = "EXC",
                          Import = 9,
                      }).ToList();
            }
            catch (Exception)
            {
            }


            try
            {
                sqlLogs = ReadLog(Path.Combine(_contentRoot, "Log", "SqlLog.log"), Encoding.UTF8)
                      .Split("--------------------------------")
                      .Where(d => !string.IsNullOrEmpty(d) && d != "\n" && d != "\r\n")
                      .Select(d => new LogInfo
                      {
                          Datetime = d.Split("|")[0].ObjToDate(),
                          Content = d.Split("|")[1]?.Replace("\r\n", "<br>"),
                          LogColor = "SQL",
                      }).ToList();
            }
            catch (Exception)
            {
            }

            try
            {
                reqresLogs = ReadLog(Path.Combine(_contentRoot, "Log", "RequestResponseLog.log"), Encoding.UTF8)
                      .Split("--------------------------------")
                      .Where(d => !string.IsNullOrEmpty(d) && d != "\n" && d != "\r\n")
                      .Select(d => new LogInfo
                      {
                          Datetime = d.Split("|")[0].ObjToDate(),
                          Content = d.Split("|")[1]?.Replace("\r\n", "<br>"),
                          LogColor = "ReqRes",
                      }).ToList();
            }
            catch (Exception)
            {
            }

            if (excLogs != null)
            {
                aopLogs.AddRange(excLogs);
            }
            if (sqlLogs != null)
            {
                aopLogs.AddRange(sqlLogs);
            }
            if (reqresLogs != null)
            {
                aopLogs.AddRange(reqresLogs);
            }
            aopLogs = aopLogs.OrderByDescending(d => d.Import).ThenByDescending(d => d.Datetime).Take(100).ToList();

            return aopLogs;
        }

    }

    public class LogInfo
    {
        public DateTime Datetime { get; set; }
        public string Content { get; set; }
        public string IP { get; set; }
        public string LogColor { get; set; }
        public int Import { get; set; } = 0;
    }
}
