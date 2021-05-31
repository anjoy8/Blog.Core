using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
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
        public static void WriteLog(string filename, string[] dataParas, bool IsHeader = true, string defaultFolder = "")
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                //.WriteTo.File(Path.Combine($"log/Serilog/{filename}/", ".log"), rollingInterval: RollingInterval.Day, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] {Message}{NewLine}{Exception}")
                .WriteTo.File(Path.Combine("Log", defaultFolder, $"{filename}.log"),
                rollingInterval: RollingInterval.Infinite,
                outputTemplate: "{Message}{NewLine}{Exception}")

                // 将日志托送到远程ES
                // docker run -p 9200:9200 -p 9300:9300 -e "discovery.type=single-node" -e ES_JAVA_OPTS="-Xms256m -Xmx256m" -d --name ES01 elasticsearch:7.2.0
                //.Enrich.FromLogContext()
                //.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://x.xxx.xx.xx:9200/"))
                //{
                //    AutoRegisterTemplate = true,
                //})

                .CreateLogger();

            var now = DateTime.Now;
            string logContent = String.Join("\r\n", dataParas);
            if (IsHeader)
            {
                logContent = (
                   "--------------------------------\r\n" +
                   DateTime.Now + "|\r\n" +
                   String.Join("\r\n", dataParas) + "\r\n"
                   );
            }

            Log.Information(logContent);
            Log.CloseAndFlush();
        }
        /// <summary>
        /// 记录异常日志
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void WriteErrorLog(string filename, string message, Exception ex)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .WriteTo.File(Path.Combine($"log/Error/{filename}/", ".txt"), rollingInterval: RollingInterval.Day)
                .CreateLogger();
            Log.Error(ex, message);
            Log.CloseAndFlush();
        }
    }
}