using Blog.Core.Common.Helper;
using Microsoft.Extensions.Hosting;
using Nacos.V2;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Core.Extensions.NacosConfig
{
    /// <summary>
    /// 
    /// </summary>
    public class NacosListenNamingTask : BackgroundService
    {
        private readonly INacosNamingService _nacosNamingService;

        /// <summary>
        /// 监听事件
        /// </summary>
        private NamingServiceEventListener eventListener = new NamingServiceEventListener();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nacosNamingService"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="configuration"></param>
        public NacosListenNamingTask(INacosNamingService nacosNamingService)
        {
            _nacosNamingService = nacosNamingService;
        }

        /// <summary>
        /// 订阅服务变化 
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Add listener
            await _nacosNamingService.Subscribe(JsonConfigSettings.NacosServiceName, Nacos.V2.Common.Constants.DEFAULT_GROUP, eventListener);
            var instance = new Nacos.V2.Naming.Dtos.Instance()
            {
                ServiceName = JsonConfigSettings.NacosServiceName,
                ClusterName = Nacos.V2.Common.Constants.DEFAULT_CLUSTER_NAME,
                Ip = IpHelper.GetCurrentIp(null),
                Port = JsonConfigSettings.NacosPort,
                Enabled = true,
                Weight = 1000,// 权重 默认1000
                Metadata = JsonConfigSettings.NacosMetadata
            };
            await _nacosNamingService.RegisterInstance(JsonConfigSettings.NacosServiceName, Nacos.V2.Common.Constants.DEFAULT_GROUP, instance);
            ConsoleHelper.WriteSuccessLine($"Nacos connect: Success!");
        }

        // 程序停止
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            // Remove listener
            await _nacosNamingService.Unsubscribe(JsonConfigSettings.NacosServiceName, Nacos.V2.Common.Constants.DEFAULT_GROUP, eventListener);
            await _nacosNamingService.DeregisterInstance(JsonConfigSettings.NacosServiceName, Nacos.V2.Common.Constants.DEFAULT_GROUP, IpHelper.GetCurrentIp(null), JsonConfigSettings.NacosPort);

            await base.StopAsync(cancellationToken);
        }
    }

    /// <summary>
    /// 服务变更事件监听
    /// </summary>
    public class NamingServiceEventListener : IEventListener
    {
        /// <summary>
        ///  
        /// </summary>
        //public static redisHelper _redisCachqManager = new redisHelper();

        /// <summary>
        /// 监听事件
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public Task OnEvent(Nacos.V2.IEvent @event)
        {
            if (@event is Nacos.V2.Naming.Event.InstancesChangeEvent e)
            {
                Console.WriteLine($"==========收到服务变更事件=======》{Newtonsoft.Json.JsonConvert.SerializeObject(e)}");

                // 配置有变动后 刷新redis配置 刷新 mq配置

                //_redisCachqManager.DisposeRedisConnection();


                Serilog.Log.Information($"收到服务变更事件!!! {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}  [{e}]");
            }

            return Task.CompletedTask;
        }
    }
}
