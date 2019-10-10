using System;
using System.IO;
using System.Reflection;
using System.Xml;
using Autofac.Extensions.DependencyInjection;
using Blog.Core.Model.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Blog.Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            XmlDocument log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead("Log4net.config"));

            var repo = log4net.LogManager.CreateRepository(
                Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));

            log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);




            // 生成承载 web 应用程序的 Microsoft.AspNetCore.Hosting.IWebHost。Build是WebHostBuilder最终的目的，将返回一个构造的WebHost，最终生成宿主。
            var host = CreateHostBuilder(args).Build();

            // 创建可用于解析作用域服务的新 Microsoft.Extensions.DependencyInjection.IServiceScope。
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();

                try
                {
                    // 从 system.IServicec提供程序获取 T 类型的服务。
                    // 为了大家的数据安全，这里先注释掉了，大家自己先测试玩一玩吧。
                    // 数据库连接字符串是在 Model 层的 Seed 文件夹下的 MyContext.cs 中
                    var configuration = services.GetRequiredService<IConfiguration>();
                    if (configuration.GetSection("AppSettings")["SeedDBEnabled"].ObjToBool())
                    {
                        var myContext = services.GetRequiredService<MyContext>();
                        DBSeed.SeedAsync(myContext).Wait();
                    }
                }
                catch (Exception e)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(e, "Error occured seeding the Database.");
                    throw;
                }
            }

            // 运行 web 应用程序并阻止调用线程, 直到主机关闭。
            // 创建完 WebHost 之后，便调用它的 Run 方法，而 Run 方法会去调用 WebHost 的 StartAsync 方法
            // 将Initialize方法创建的Application管道传入以供处理消息
            // 执行HostedServiceExecutor.StartAsync方法
            host.Run();
        }

        //public static IHostBuilder CreateHostBuilder2(string[] args) =>
        //    //使用预配置的默认值初始化 Microsoft.AspNetCore.Hosting.WebHostBuilder 类的新实例。
        //    WebHost.CreateDefaultBuilder(args)
        //        //指定要由 web 主机使用的启动类型。相当于注册了一个IStartup服务。可以自定义启动服务，比如.UseStartup(typeof(StartupDevelopment).GetTypeInfo().Assembly.FullName)
        //        .UseUrls("http://localhost:8081")
        //        .UseStartup<Startup>();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
          Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory()) //<--NOTE THIS
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                .ConfigureKestrel(serverOptions =>
                {
                    serverOptions.AllowSynchronousIO = true;//启用同步 IO
                })
                .UseStartup<Startup>()
                .UseUrls("http://localhost:8081")
                .ConfigureLogging((hostingContext, builder) =>
                {
                    builder.ClearProviders();
                    builder.SetMinimumLevel(LogLevel.Trace);
                    builder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    builder.AddConsole();
                    builder.AddDebug();
                });
            });
    }
}
