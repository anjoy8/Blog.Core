using Blog.Core.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using Blog.Core.Common.Https;
using Blog.Core.Common.Option;
using Blog.Core.Common.Swagger;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Profiling;
using StackExchange.Profiling.SqlFormatters;
using StackExchange.Profiling.Storage;
using StackExchange.Redis;

namespace Blog.Core.Extensions
{
    /// <summary>
    /// MiniProfiler 启动服务
    /// </summary>
    public static class MiniProfilerSetup
    {
        public static void AddMiniProfilerSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (!AppSettings.app("Startup", "MiniProfiler", "Enabled").ObjToBool()) return;

            //使用MiniProfiler
            services.AddMiniProfiler(options =>
                {
                    //访问地址路由根目录；默认为：/mini-profiler-resources
                    //options.RouteBasePath = "/profiler";
                    //数据缓存时间
                    //获取redis配置
                    var redisOptions = App.GetOptions<RedisOptions>();
                    if (redisOptions.Enable)
                        options.Storage =
                            new RedisStorage((ConnectionMultiplexer)App.GetService<IConnectionMultiplexer>());
                    else
                        options.Storage = new MemoryCacheStorage(App.GetService<IMemoryCache>(), TimeSpan.FromMinutes(60));

                    //sql格式化设置
                    options.SqlFormatter = new InlineFormatter();
                    //跟踪连接打开关闭
                    options.TrackConnectionOpenClose = true;
                    //界面主题颜色方案;默认浅色
                    options.ColorScheme = ColorScheme.Dark;
                    //.net core 3.0以上：对MVC过滤器进行分析
                    options.EnableMvcFilterProfiling = true;
                    //对视图进行分析
                    options.EnableMvcViewProfiling = true;

                    //控制访问页面授权，默认所有人都能访问
                    //options.ResultsAuthorize;
                    //要控制分析哪些请求，默认说有请求都分析
                    //options.ShouldProfile

                    //内部异常处理
                    //options.OnInternalError = e => MyExceptionLogger(e);
                    //options.RouteBasePath = "/profiler";

                    //(options.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(10);
                    options.PopupRenderPosition = RenderPosition.Left;
                    options.PopupShowTimeWithChildren = true;

                    //只监控api 接口
                    options.ShouldProfile = ShouldProfile;

                    // 可以增加权限
                    // options.ResultsAuthorize = request =>
                    // {
                    //     if (request.IsLocal()) return true;
                    //
                    //     var path = request.HttpContext.Request.Path.Value;
                    //     if (path == null || !path.StartsWith(options.RouteBasePath)) return true;
                    //
                    //     var flag = request.HttpContext.IsSuccessSwagger();
                    //     if (!flag) request.HttpContext.RedirectSwaggerLogin();
                    //     return flag;
                    // };
                }
            );
        }

        private static bool ShouldProfile(HttpRequest request)
        {
            return request.Path.StartsWithSegments("/api");
        }
    }
}