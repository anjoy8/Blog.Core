using Blog.Core.Common;
using Blog.Core.Common.Helper;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Blog.Core.Extensions
{
    /// <summary>
    /// Cors 启动服务
    /// </summary>
    public static class AppConfigSetup
    {
        public static void AddAppConfigSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            if (Appsettings.app(new string[] { "Startup", "AppConfigAlert", "Enabled" }).ObjToBool())
            {
                Console.WriteLine("************ Blog.Core Config Set *****************");
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
                if (!Appsettings.app(new string[] { "AppSettings", "RedisCachingAOP", "Enabled" }).ObjToBool())
                {
                    Console.WriteLine($"Redis Caching AOP: False");
                }
                else
                {
                    ConsoleHelper.WriteSuccessLine($"Redis Caching AOP: True");
                }

                // 内存缓存AOP
                if (!Appsettings.app(new string[] { "AppSettings", "MemoryCachingAOP", "Enabled" }).ObjToBool())
                {
                    Console.WriteLine($"Memory Caching AOP: False");
                }
                else
                {
                    ConsoleHelper.WriteSuccessLine($"Memory Caching AOP: True");
                }

                // 服务日志AOP
                if (!Appsettings.app(new string[] { "AppSettings", "LogAOP", "Enabled" }).ObjToBool())
                {
                    Console.WriteLine($"Service Log AOP: False");
                }
                else
                {
                    ConsoleHelper.WriteSuccessLine($"Service Log AOP: True");
                }

                // 事务AOP
                if (!Appsettings.app(new string[] { "AppSettings", "TranAOP", "Enabled" }).ObjToBool())
                {
                    Console.WriteLine($"Transaction AOP: False");
                }
                else
                {
                    ConsoleHelper.WriteSuccessLine($"Transaction AOP: True");
                }

                // 数据库Sql执行AOP
                if (!Appsettings.app(new string[] { "AppSettings", "SqlAOP", "Enabled" }).ObjToBool())
                {
                    Console.WriteLine($"DB Sql AOP: False");
                }
                else
                {
                    ConsoleHelper.WriteSuccessLine($"DB Sql AOP: True");
                }

                // SingnalR发送数据
                if (!Appsettings.app(new string[] { "Middleware", "SignalR", "Enabled" }).ObjToBool())
                {
                    Console.WriteLine($"SignalR send data: False");
                }
                else
                {
                    ConsoleHelper.WriteSuccessLine($"SignalR send data: True");
                }

                // IP限流
                if (!Appsettings.app(new string[] { "IpRateLimiting", "EnableEndpointRateLimiting" }).ObjToBool())
                {
                    Console.WriteLine($"IpRateLimiting: False");
                }
                else
                {
                    ConsoleHelper.WriteSuccessLine($"IpRateLimiting: True");
                }

                Console.WriteLine();
            }

        }
    }
}
