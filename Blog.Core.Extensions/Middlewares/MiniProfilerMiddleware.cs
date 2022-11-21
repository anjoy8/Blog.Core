using System;
using Blog.Core.Common;
using log4net;
using Microsoft.AspNetCore.Builder;

namespace Blog.Core.Extensions.Middlewares
{
    /// <summary>
    /// MiniProfiler性能分析
    /// </summary>
    public static class MiniProfilerMiddleware
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MiniProfilerMiddleware));
        public static void UseMiniProfilerMiddleware(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            try
            {
                if (AppSettings.app("Startup", "MiniProfiler", "Enabled").ObjToBool())
                { 
                    // 性能分析
                    app.UseMiniProfiler();

                }
            }
            catch (Exception e)
            {
                Log.Error($"An error was reported when starting the MiniProfilerMildd.\n{e.Message}");
                throw;
            }
        }
    }
}
