using System;
using Ocelot.ServiceDiscovery;
using Microsoft.Extensions.DependencyInjection;
using Nacos.V2;
using Ocelot.Provider.Nacos.NacosClient.V2;
using Microsoft.Extensions.Options;

namespace Ocelot.Provider.Nacos
{
    public static class NacosProviderFactory
    {
        public static ServiceDiscoveryFinderDelegate Get = (provider, config, route) =>
        {
            var client = provider.GetService<INacosNamingService>();
            if (config.Type?.ToLower() == "nacos" && client != null)
            {
                var option = provider.GetService<IOptions<NacosAspNetOptions>>();
                return new Nacos(route.ServiceName, client, option);
            }
            return null;
        };
    }
}
