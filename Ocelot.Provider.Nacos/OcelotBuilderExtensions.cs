using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Nacos.NacosClient.V2;
using Ocelot.ServiceDiscovery;

namespace Ocelot.Provider.Nacos
{
    public static class OcelotBuilderExtensions
    {
        public static IOcelotBuilder AddNacosDiscovery(this IOcelotBuilder builder)
        {
            builder.Services.AddNacosAspNet(builder.Configuration);
            builder.Services.AddSingleton<ServiceDiscoveryFinderDelegate>(NacosProviderFactory.Get);
            builder.Services.AddSingleton<OcelotMiddlewareConfigurationDelegate>(NacosMiddlewareConfigurationProvider.Get);
            return builder;
        }
    }
}
