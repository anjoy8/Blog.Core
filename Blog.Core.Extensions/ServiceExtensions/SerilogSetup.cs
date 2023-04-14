using Blog.Core.Common;
using Blog.Core.Common.LogHelper;
using Blog.Core.Serilog.Configuration;
using Blog.Core.Serilog.Extensions;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Debugging;
using Serilog.Events;
using System;
using System.IO;

namespace Blog.Core.Extensions.ServiceExtensions;

public static class SerilogSetup
{
    public static IHostBuilder AddSerilogSetup(this IHostBuilder host)
    {
        if (host == null) throw new ArgumentNullException(nameof(host));

        var loggerConfiguration = new LoggerConfiguration()
            .ReadFrom.Configuration(AppSettings.Configuration)
            .Enrich.FromLogContext()
            //输出到控制台
            .WriteToConsole()
            //将日志保存到文件中
            .WriteToFile()
            //配置日志库
            .WriteToLogBatching();

        //配置Seq日志中心
        if (AppSettings.app("Seq", "Enabled").ObjToBool())
        {
            var address = AppSettings.app("Seq", "Address");
            var apiKey = AppSettings.app("Seq", "ApiKey");
            if (!address.IsNullOrEmpty())
            {
                loggerConfiguration =
                    loggerConfiguration.WriteTo.Seq(address, restrictedToMinimumLevel: LogEventLevel.Verbose,
                        apiKey: apiKey, eventBodyLimitBytes: 10485760);
            }
        }

        Log.Logger = loggerConfiguration.CreateLogger();

        //Serilog 内部日志
        var file = File.CreateText(LogContextStatic.Combine($"SerilogDebug{DateTime.Now:yyyyMMdd}.txt"));
        SelfLog.Enable(TextWriter.Synchronized(file));

        host.UseSerilog();
        return host;
    }
}