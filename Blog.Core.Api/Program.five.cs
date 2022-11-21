//using Autofac.Extensions.DependencyInjection;
//using Blog.Core.Extensions.Apollo;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using System;
//using System.IO;

// 这是asp.net5.0的写法，如果用5.0，请用本文件代码替换Program.cs代码
//namespace Blog.Core
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            //初始化默认主机Builder
//            Host.CreateDefaultBuilder(args)
//             .UseServiceProviderFactory(new AutofacServiceProviderFactory())
//             .ConfigureWebHostDefaults(webBuilder =>
//             {
//                 webBuilder
//                 .UseStartup<Startup>()
//                 .ConfigureAppConfiguration((hostingContext, config) =>
//                 {
//                     config.Sources.Clear();
//                     config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
//                           //.AddJsonFile($"appsettings{ GetAppSettingsConfigName() }json", optional: true, reloadOnChange: false)
//                           ;
//                     //接入Apollo配置中心
//                     config.AddConfigurationApollo("appsettings.apollo.json");
//                 })
//                 .UseUrls("http://*:9291")
//                 .ConfigureLogging((hostingContext, builder) =>
//                 {
//                     // 1.过滤掉系统默认的一些日志
//                     builder.AddFilter("System", LogLevel.Error);
//                     builder.AddFilter("Microsoft", LogLevel.Error);

//                     // 2.也可以在appsettings.json中配置，LogLevel节点

//                     // 3.统一设置
//                     builder.SetMinimumLevel(LogLevel.Error);

//                     // 默认log4net.confg
//                     builder.AddLog4Net(Path.Combine(Directory.GetCurrentDirectory(), "Log4net.config"));
//                 })
//                 ;
//             })
//            // 生成承载 web 应用程序的 Microsoft.AspNetCore.Hosting.IWebHost。Build是WebHostBuilder最终的目的，将返回一个构造的WebHost，最终生成宿主。
//             .Build()
//            // 运行 web 应用程序并阻止调用线程, 直到主机关闭。
//            // ※※※※ 有异常，查看 Log 文件夹下的异常日志 ※※※※  
//             .Run();
//        }


//        /// <summary>
//        /// 根据环境变量定向配置文件名称
//        /// </summary>
//        /// <returns></returns>
//        private static string GetAppSettingsConfigName()
//        {
//            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != null
//                      && Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "")
//            {
//                return $".{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.";
//            }
//            else
//            {
//                return ".";
//            }
//        }
//    }
//}
