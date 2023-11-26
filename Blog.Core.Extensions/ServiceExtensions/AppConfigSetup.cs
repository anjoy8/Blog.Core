using Blog.Core.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using Blog.Core.Common.DB;

namespace Blog.Core.Extensions
{
    /// <summary>
    /// 项目 启动服务
    /// </summary>
    public static class AppConfigSetup
    {
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
                    new string[] { "读写分离", BaseDBConfig.MainConfig.SlaveConnectionConfigs.AnyNoException()? "True" : "False" },
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
                    new string[] { "缓存AOP", AppSettings.app("AppSettings", "CachingAOP", "Enabled") },
                    new string[] { "服务日志AOP", AppSettings.app("AppSettings", "LogAOP", "Enabled") },
                    new string[] { "事务AOP", AppSettings.app("AppSettings", "TranAOP", "Enabled") },
                    new string[] { "服务审计AOP", AppSettings.app("AppSettings", "UserAuditAOP", "Enabled") },
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