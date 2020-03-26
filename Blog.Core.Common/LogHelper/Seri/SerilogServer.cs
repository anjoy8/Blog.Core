using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.IO;

namespace Blog.Core.Common.LogHelper
{
    public class SerilogServer 
    {
        /// <summary>
        /// 记录日常日志
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="message"></param>
        /// <param name="info"></param>
        public static void WriteLog(string filename, string message, string info)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .WriteTo.File(Path.Combine($"log/Information/{filename}/", ".txt"), rollingInterval: RollingInterval.Day)
                .CreateLogger();
            Log.Information(message+ info, info);
            Log.CloseAndFlush();
        }
        /// <summary>
        /// 记录异常日志
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void WriteErrorLog(string filename,string message, Exception ex)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .WriteTo.File(Path.Combine($"log/Error/{filename}/",".txt"),rollingInterval:RollingInterval.Day)
                .CreateLogger();
            Log.Error(ex, message);
            Log.CloseAndFlush();
        }
    }
}
