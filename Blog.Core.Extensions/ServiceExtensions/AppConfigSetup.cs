using Blog.Core.Common;
using Blog.Core.Common.Helper;
using Blog.Core.Common.LogHelper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Core.Extensions
{
    /// <summary>
    /// 项目 启动服务
    /// </summary>
    public static class AppConfigSetup
    {
        public static void AddAppConfigSetup(this IServiceCollection services, IHostEnvironment env)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            if (AppSettings.app(new string[] { "Startup", "AppConfigAlert", "Enabled" }).ObjToBool())
            {
                if (env.IsDevelopment())
                {
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    Console.OutputEncoding = Encoding.GetEncoding("GB2312");
                }

                Console.WriteLine("************ Blog.Core Config Set *****************");

                ConsoleHelper.WriteSuccessLine("Current environment: " + Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));

                // 授权策略方案
                if (Permissions.IsUseIds4)
                {
                    ConsoleHelper.WriteSuccessLine($"Current authorization scheme: " + (Permissions.IsUseIds4 ? "Ids4" : "JWT"));
                }
                else
                {
                    Console.WriteLine($"Current authorization scheme: " + (Permissions.IsUseIds4 ? "Ids4" : "JWT"));
                }

                // Redis缓存AOP
                if (!AppSettings.app(new string[] { "AppSettings", "RedisCachingAOP", "Enabled" }).ObjToBool())
                {
                    Console.WriteLine($"Redis Caching AOP: False");
                }
                else
                {
                    ConsoleHelper.WriteSuccessLine($"Redis Caching AOP: True");
                }

                // 内存缓存AOP
                if (!AppSettings.app(new string[] { "AppSettings", "MemoryCachingAOP", "Enabled" }).ObjToBool())
                {
                    Console.WriteLine($"Memory Caching AOP: False");
                }
                else
                {
                    ConsoleHelper.WriteSuccessLine($"Memory Caching AOP: True");
                }

                // 服务日志AOP
                if (!AppSettings.app(new string[] { "AppSettings", "LogAOP", "Enabled" }).ObjToBool())
                {
                    Console.WriteLine($"Service Log AOP: False");
                }
                else
                {
                    ConsoleHelper.WriteSuccessLine($"Service Log AOP: True");
                }

                // 开启的中间件日志
                var requestResponseLogOpen = AppSettings.app(new string[] { "Middleware", "RequestResponseLog", "Enabled" }).ObjToBool();
                var ipLogOpen = AppSettings.app(new string[] { "Middleware", "IPLog", "Enabled" }).ObjToBool();
                var recordAccessLogsOpen = AppSettings.app(new string[] { "Middleware", "RecordAccessLogs", "Enabled" }).ObjToBool();
                ConsoleHelper.WriteSuccessLine($"OPEN Log: " +
                                               (requestResponseLogOpen ? "RequestResponseLog √," : "") +
                                               (ipLogOpen ? "IPLog √," : "") +
                                               (recordAccessLogsOpen ? "RecordAccessLogs √," : "")
                );

                // 事务AOP
                if (!AppSettings.app(new string[] { "AppSettings", "TranAOP", "Enabled" }).ObjToBool())
                {
                    Console.WriteLine($"Transaction AOP: False");
                }
                else
                {
                    ConsoleHelper.WriteSuccessLine($"Transaction AOP: True");
                }

                // 数据库Sql执行AOP
                if (!AppSettings.app(new string[] { "AppSettings", "SqlAOP", "OutToLogFile", "Enabled" }).ObjToBool())
                {
                    Console.WriteLine($"DB Sql AOP To LogFile: False");
                }
                else
                {
                    ConsoleHelper.WriteSuccessLine($"DB Sql AOP To LogFile: True");
                }

                // Sql执行日志输出到控制台
                if (!AppSettings.app(new string[] { "AppSettings", "SqlAOP", "OutToConsole", "Enabled" }).ObjToBool())
                {
                    Console.WriteLine($"DB Sql AOP To Console: False");
                }
                else
                {
                    ConsoleHelper.WriteSuccessLine($"DB Sql AOP To Console: True");
                }

                // SingnalR发送数据
                if (!AppSettings.app(new string[] { "Middleware", "SignalR", "Enabled" }).ObjToBool())
                {
                    Console.WriteLine($"SignalR send data: False");
                }
                else
                {
                    ConsoleHelper.WriteSuccessLine($"SignalR send data: True");
                }

                // IP限流
                if (!AppSettings.app("Middleware", "IpRateLimit", "Enabled").ObjToBool())
                {
                    Console.WriteLine($"IpRateLimiting: False");
                }
                else
                {
                    ConsoleHelper.WriteSuccessLine($"IpRateLimiting: True");
                }

                // 性能分析
                if (!AppSettings.app("Startup", "MiniProfiler", "Enabled").ObjToBool())
                {
                    Console.WriteLine($"MiniProfiler: False");
                }
                else
                {
                    ConsoleHelper.WriteSuccessLine($"MiniProfiler: True");
                }

                // CORS跨域
                if (!AppSettings.app("Startup", "Cors", "EnableAllIPs").ObjToBool())
                {
                    Console.WriteLine($"EnableAllIPs For CORS: False");
                }
                else
                {
                    ConsoleHelper.WriteSuccessLine($"EnableAllIPs For CORS: True");
                }

                // redis消息队列
                if (!AppSettings.app("Startup", "RedisMq", "Enabled").ObjToBool())
                {
                    Console.WriteLine($"Redis MQ: False");
                }
                else
                {
                    ConsoleHelper.WriteSuccessLine($"Redis MQ: True");
                }

                // RabbitMQ 消息队列
                if (!AppSettings.app("RabbitMQ", "Enabled").ObjToBool())
                {
                    Console.WriteLine($"RabbitMQ: False");
                }
                else
                {
                    ConsoleHelper.WriteSuccessLine($"RabbitMQ: True");
                }

                // Consul 注册服务
                if (!AppSettings.app("Middleware", "Consul", "Enabled").ObjToBool())
                {
                    Console.WriteLine($"Consul service: False");
                }
                else
                {
                    ConsoleHelper.WriteSuccessLine($"Consul service: True");
                }

                // EventBus 事件总线
                if (!AppSettings.app("EventBus", "Enabled").ObjToBool())
                {
                    Console.WriteLine($"EventBus: False");
                }
                else
                {
                    ConsoleHelper.WriteSuccessLine($"EventBus: True");
                }

                // 多库
                if (!AppSettings.app(new string[] { "MutiDBEnabled" }).ObjToBool())
                {
                    Console.WriteLine($"Is multi-DataBase: False");
                }
                else
                {
                    ConsoleHelper.WriteSuccessLine($"Is multi-DataBase: True");
                }

                // 读写分离
                if (!AppSettings.app(new string[] { "CQRSEnabled" }).ObjToBool())
                {
                    Console.WriteLine($"Is CQRS: False");
                }
                else
                {
                    ConsoleHelper.WriteSuccessLine($"Is CQRS: True");
                }

                Console.WriteLine();
            }
        }

        public static void AddAppTableConfigSetup(this IServiceCollection services, IHostEnvironment env)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            if (AppSettings.app(new string[] { "Startup", "AppConfigAlert", "Enabled" }).ObjToBool())
            {
                if (env.IsDevelopment())
                {
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    Console.OutputEncoding = Encoding.GetEncoding("GB2312");
                }

                #region 程序配置

                List<string[]> configInfos = new()
                {
                    new string[] { "当前环境", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") },
                    new string[] { "当前的授权方案", Permissions.IsUseIds4 ? "Ids4" : "JWT" },
                    new string[] { "CORS跨域", AppSettings.app("Startup", "Cors", "EnableAllIPs") },
                    new string[] { "RabbitMQ消息列队", AppSettings.app("RabbitMQ", "Enabled") },
                    new string[] { "事件总线(必须开启消息列队)", AppSettings.app("EventBus", "Enabled") },
                    new string[] { "redis消息队列", AppSettings.app("Startup", "RedisMq", "Enabled") },
                    new string[] { "是否多库", AppSettings.app("MutiDBEnabled") },
                    new string[] { "读写分离", AppSettings.app("CQRSEnabled") },
                };

                new ConsoleTable()
                {
                    TitleString = "Blog.Core 配置集",
                    Columns = new string[] { "配置名称", "配置信息/是否启动" },
                    Rows = configInfos,
                    EnableCount = false,
                    Alignment = Alignment.Left,
                    ColumnBlankNum = 4,
                    TableStyle = TableStyle.Alternative
                }.Writer(ConsoleColor.Blue);
                Console.WriteLine();

                #endregion 程序配置

                #region AOP

                List<string[]> aopInfos = new()
                {
                    new string[] { "Redis缓存AOP", AppSettings.app("AppSettings", "RedisCachingAOP", "Enabled") },
                    new string[] { "内存缓存AOP", AppSettings.app("AppSettings", "MemoryCachingAOP", "Enabled") },
                    new string[] { "服务日志AOP", AppSettings.app("AppSettings", "LogAOP", "Enabled") },
                    new string[] { "事务AOP", AppSettings.app("AppSettings", "TranAOP", "Enabled") },
                    new string[] { "Sql执行AOP", AppSettings.app("AppSettings", "SqlAOP", "Enabled") },
                    new string[] { "Sql执行AOP控制台输出", AppSettings.app("AppSettings", "SqlAOP", "LogToConsole", "Enabled") },
                };

                new ConsoleTable
                {
                    TitleString = "AOP",
                    Columns = new string[] { "配置名称", "配置信息/是否启动" },
                    Rows = aopInfos,
                    EnableCount = false,
                    Alignment = Alignment.Left,
                    ColumnBlankNum = 7,
                    TableStyle = TableStyle.Alternative
                }.Writer(ConsoleColor.Blue);
                Console.WriteLine();

                #endregion AOP

                #region 中间件

                List<string[]> MiddlewareInfos = new()
                {
                    new string[] { "请求纪录中间件", AppSettings.app("Middleware", "RecordAccessLogs", "Enabled") },
                    new string[] { "IP记录中间件", AppSettings.app("Middleware", "IPLog", "Enabled") },
                    new string[] { "请求响应日志中间件", AppSettings.app("Middleware", "RequestResponseLog", "Enabled") },
                    new string[] { "SingnalR实时发送请求数据中间件", AppSettings.app("Middleware", "SignalR", "Enabled") },
                    new string[] { "IP限流中间件", AppSettings.app("Middleware", "IpRateLimit", "Enabled") },
                    new string[] { "性能分析中间件", AppSettings.app("Startup", "MiniProfiler", "Enabled") },
                    new string[] { "Consul注册服务", AppSettings.app("Middleware", "Consul", "Enabled") },
                };

                new ConsoleTable
                {
                    TitleString = "中间件",
                    Columns = new string[] { "配置名称", "配置信息/是否启动" },
                    Rows = MiddlewareInfos,
                    EnableCount = false,
                    Alignment = Alignment.Left,
                    ColumnBlankNum = 3,
                    TableStyle = TableStyle.Alternative
                }.Writer(ConsoleColor.Blue);
                Console.WriteLine();

                #endregion 中间件
            }
        }
    }
}