using Blog.Core.Common;
using Blog.Core.IServices;
using Blog.Core.Tasks;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Blog.Core.Middlewares
{
    /// <summary>
    /// 中间件
    /// 记录IP请求数据
    /// </summary>
    public class QuartzNetJobMildd
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly RequestDelegate _next;
        private readonly ITasksQzServices _tasksQzServices;
        private readonly ISchedulerCenter _schedulerCenter;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="tasksQzServices"></param>
        /// <param name="schedulerCenter"></param>
        public QuartzNetJobMildd(RequestDelegate next, ITasksQzServices tasksQzServices, ISchedulerCenter schedulerCenter)
        {
            _next = next;
            _tasksQzServices = tasksQzServices;
            _schedulerCenter = schedulerCenter;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (Appsettings.app("Middleware", "QuartzNetJob", "Enabled").ObjToBool())
            {

                var allQzServices = await _tasksQzServices.Query();
                foreach (var item in allQzServices)
                {
                    if (item.IsStart)
                    {
                        var ResuleModel = await _schedulerCenter.AddScheduleJobAsync(item);
                        if (ResuleModel.success)
                        {
                            Console.WriteLine($"QuartzNetJob{item.Name}启动成功！");
                        }
                    }
                }

                await _next(context);
            }
            else
            {
                await _next(context);
            }
        }
    }
}

