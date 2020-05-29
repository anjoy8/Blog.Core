using Blog.Core.Common;
using Blog.Core.IServices;
using Blog.Core.Model.Models;
using Blog.Core.Tasks;
using log4net;
using Microsoft.AspNetCore.Builder;
using System;
using System.Threading.Tasks;

namespace Blog.Core.Extensions
{
    /// <summary>
    /// Quartz 启动服务
    /// </summary>
    public static class QuartzJobMildd
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(QuartzJobMildd));
        public static void UseQuartzJobMildd(this IApplicationBuilder app, ITasksQzServices tasksQzServices, ISchedulerCenter schedulerCenter)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            try
            {
                if (Appsettings.app("Middleware", "QuartzNetJob", "Enabled").ObjToBool())
                {

                    var allQzServices = tasksQzServices.Query().Result;
                    foreach (var item in allQzServices)
                    {
                        if (item.IsStart)
                        {
                            var ResuleModel = schedulerCenter.AddScheduleJobAsync(item).Result;
                            if (ResuleModel.success)
                            {
                                Console.WriteLine($"QuartzNetJob{item.Name}启动成功！");
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                log.Error($"An error was reported when starting the job service.\n{e.Message}");
                throw;
            }
        }
    }
}
