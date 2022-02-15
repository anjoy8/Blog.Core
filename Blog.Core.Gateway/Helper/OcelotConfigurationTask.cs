
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Nacos.V2;
using System;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Common.Helper;
using Ocelot.Configuration.Repository;
using Ocelot.Configuration.Creator;
using Newtonsoft.Json.Linq;
using Ocelot.Configuration.File;
using Blog.Core.Common;

namespace ApiGateway.Helper
{
    /// <summary>
    /// Nacos配置文件变更事件
    /// </summary>
    public class OcelotConfigurationTask : BackgroundService
    {
        private readonly INacosConfigService _configClient;
        private readonly INacosNamingService _servClient;
        /// <summary>
        /// Nacos 配置文件监听事件
        /// </summary>
        private OcelotConfigListener nacosConfigListener = new OcelotConfigListener();
        private AppConfigListener AppConfigListener = new AppConfigListener();
        private string OcelotConfig = "";
        private string OcelotConfigGroup = "";
        private string AppConfig = "";
        private string AppConfigGroup = "";


        /// <summary>
        /// 
        /// </summary>
        /// <param name="serv"></param>
        /// <param name="configClient"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="_internalConfigurationRepo"></param>
        /// <param name="_internalConfigurationCreator"></param>
        public OcelotConfigurationTask(INacosNamingService serv, INacosConfigService configClient, IServiceProvider serviceProvider, IInternalConfigurationRepository _internalConfigurationRepo, IInternalConfigurationCreator _internalConfigurationCreator)
        {
            _configClient = configClient;
            _servClient = serv;
            nacosConfigListener.internalConfigurationRepo = _internalConfigurationRepo;
            nacosConfigListener.internalConfigurationCreator = _internalConfigurationCreator;
            OcelotConfig = Appsettings.GetValue("ApiGateWay:OcelotConfig");
            OcelotConfigGroup = Appsettings.GetValue("ApiGateWay:OcelotConfigGroup");
            AppConfig = Appsettings.GetValue("ApiGateWay:AppConfig");
            AppConfigGroup = Appsettings.GetValue("ApiGateWay:AppConfigGroup");
            



            string OcelotCfg = configClient.GetConfig(OcelotConfig, OcelotConfigGroup, 10000).GetAwaiter().GetResult();
            nacosConfigListener.ReceiveConfigInfo(OcelotCfg);
            string AppCfg= configClient.GetConfig(AppConfig, AppConfigGroup, 10000).GetAwaiter().GetResult();
            AppConfigListener.ReceiveConfigInfo(AppCfg);
            //string sss = serv.Cof_NaoceGet("fld-cloud-datax", "DEFAULT_GROUP", "/api/base/deviceList?limit=10&page=1").GetAwaiter().GetResult();
        }

        

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                // Add listener OcelotConfig.json"
                await _configClient.AddListener(OcelotConfig, OcelotConfigGroup, nacosConfigListener);
                await _configClient.AddListener(AppConfig, AppConfigGroup, AppConfigListener);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            // Remove listener
            await _configClient.RemoveListener(OcelotConfig, OcelotConfigGroup, nacosConfigListener);
            await _configClient.RemoveListener(AppConfig, AppConfigGroup, AppConfigListener);
            await base.StopAsync(cancellationToken);
        }
    }

    /// <summary>
    /// 配置监听事件
    /// </summary>
    public class OcelotConfigListener : IListener
    {
        public IInternalConfigurationRepository internalConfigurationRepo { get; set; }
        public IInternalConfigurationCreator internalConfigurationCreator { get; set; }
        /// <summary>
        /// 收到配置文件变更
        /// </summary>
        /// <param name="configInfo"></param>
        public void ReceiveConfigInfo(string configInfo)
        {
            Task.Run(async () =>
            {
                FileConfiguration filecfg = JToken.Parse(configInfo).ToObject<FileConfiguration>();
                var internalConfiguration = await internalConfigurationCreator.Create(filecfg);
                if (!internalConfiguration.IsError)
                {
                   
                    internalConfigurationRepo.AddOrReplace(internalConfiguration.Data);                    
                }
            });

           
        }
    }

    public class AppConfigListener : IListener
    {
        public void ReceiveConfigInfo(string configInfo)
        {
            var _configurationBuilder = new ConfigurationBuilder();
            _configurationBuilder.Sources.Clear();
            var buffer = System.Text.Encoding.Default.GetBytes(configInfo);
            System.IO.MemoryStream ms = new System.IO.MemoryStream(buffer);
            _configurationBuilder.AddJsonStream(ms);
            var configuration = _configurationBuilder.Build();
            ms.Dispose();

           

            // 读取配置  将nacos配置中心读取到的配置 替换掉.net core 内存中的 configuration
            // 当前监听到配置配置 应该重新断开 重连 刷新等一些中间件操作
            // 比如 mq redis  等其他跟配置相关的中间件
            JsonConfigSettings.Configuration = configuration;
            Appsettings.Configuration = configuration;
        }
    }
}
