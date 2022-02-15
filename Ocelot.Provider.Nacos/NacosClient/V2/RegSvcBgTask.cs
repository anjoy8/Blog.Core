using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nacos.V2;
using Nacos.V2.Naming.Core;
using Nacos.V2.Naming.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ocelot.Provider.Nacos.NacosClient.V2
{
    public class RegSvcBgTask
    {
        private static readonly string MetadataNetVersion = "DOTNET_VERSION";
        private static readonly string MetadataHostOs = "HOST_OS";
        private static readonly string MetadataSecure = "secure";

        private readonly ILogger _logger;
        private readonly INacosNamingService _svc;
        private readonly IFeatureCollection _features;
        private NacosAspNetOptions _options;

        private IEnumerable<Uri> uris = null;

        public RegSvcBgTask(
            ILoggerFactory loggerFactory,
            INacosNamingService svc,
            IServer server,
            IOptionsMonitor<NacosAspNetOptions> optionsAccs)
        {
            _logger = loggerFactory.CreateLogger<RegSvcBgTask>();
            _svc = svc;
            _options = optionsAccs.CurrentValue;
            _features = server.Features;
        }

        public async Task StartAsync()
        {
            if (!_options.RegisterEnabled)
            {
                _logger.LogInformation("setting RegisterEnabled to false, will not register to nacos");
                return;
            }

            uris = UriTool.GetUri(_features, _options.Ip, _options.Port, _options.PreferredNetworks);

            var metadata = new Dictionary<string, string>()
            {
                { PreservedMetadataKeys.REGISTER_SOURCE, $"ASPNET_CORE" },
                { MetadataNetVersion, Environment.Version.ToString() },
                { MetadataHostOs, Environment.OSVersion.ToString() },
            };

            if (_options.Secure) metadata[MetadataSecure] = "true";

            foreach (var item in _options.Metadata)
            {
                if (!metadata.ContainsKey(item.Key))
                {
                    metadata.TryAdd(item.Key, item.Value);
                }
            }

            foreach (var uri in uris)
            {
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        var instance = new Instance
                        {
                            Ephemeral = _options.Ephemeral,
                            ServiceName = _options.ServiceName,
                            ClusterName = _options.ClusterName,
                            Enabled = _options.InstanceEnabled,
                            Healthy = true,
                            Ip = uri.Host,
                            Port = uri.Port,
                            Weight = _options.Weight,
                            Metadata = metadata,
                            InstanceId = ""
                        };

                        _logger.LogInformation("register instance to nacos server, 【{0}】", instance);

                        await _svc.RegisterInstance(_options.ServiceName, _options.GroupName, instance);
                        break;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "register instance error, count = {0}", i + 1);
                    }
                }
            }
        }

        public async Task StopAsync()
        {
            if (_options.RegisterEnabled)
            {
                _logger.LogWarning("deregister instance from nacos server, serviceName={0}", _options.ServiceName);

                foreach (var uri in uris)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        try
                        {
                            _logger.LogWarning("begin to remove instance");
                            await _svc.DeregisterInstance(_options.ServiceName, _options.GroupName, uri.Host, uri.Port, _options.ClusterName);
                            _logger.LogWarning("removed instance");
                            break;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "deregister instance error, count = {0}", i + 1);
                        }
                    }
                }
            }
        }
    }
}
