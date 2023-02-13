using System;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Common;
using Blog.Core.IServices;
using Blog.Core.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Blog.Core.Extensions.HostedService;

public class QuartzJobHostedService : IHostedService
{
    private readonly ITasksQzServices _tasksQzServices;
    private readonly ISchedulerCenter _schedulerCenter;
    private readonly ILogger<QuartzJobHostedService> _logger;

    public QuartzJobHostedService(ITasksQzServices tasksQzServices, ISchedulerCenter schedulerCenter, ILogger<QuartzJobHostedService> logger)
    {
        _tasksQzServices = tasksQzServices;
        _schedulerCenter = schedulerCenter;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Start QuartzJob Service!");
        await DoWork();
    }

    private async Task DoWork()
    {
        try
        {
            if (AppSettings.app("Middleware", "QuartzNetJob", "Enabled").ObjToBool())
            {
                var allQzServices = await _tasksQzServices.Query();
                foreach (var item in allQzServices)
                {
                    if (item.IsStart)
                    {
                        var result = await _schedulerCenter.AddScheduleJobAsync(item);
                        if (result.success)
                        {
                            Console.WriteLine($"QuartzNetJob{item.Name}启动成功！");
                        }
                        else
                        {
                            Console.WriteLine($"QuartzNetJob{item.Name}启动失败！错误信息：{result.msg}");
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error was reported when starting the job service.");
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stop QuartzJob Service!");
        return Task.CompletedTask;
    }
}