using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nacos.V2.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Ocelot.Provider.Nacos.NacosClient.V2
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add Nacos AspNet. This will register and de-register instance automatically.
        /// Mainly for nacos server 2.x
        /// </summary>
        /// <param name="services">services.</param>
        /// <param name="configuration">configuration</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddNacosAspNet(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<NacosAspNetOptions>(configuration.GetSection("nacos"));
            services.AddNacosV2Naming(configuration);
            services.AddSingleton<RegSvcBgTask>();

            return services;
        }

        /// <summary>
        /// Add Nacos AspNet. This will register and de-register instance automatically.
        /// Mainly for nacos server 2.x
        /// </summary>
        /// <param name="services">services</param>
        /// <param name="optionsAccs">optionsAccs</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddNacosAspNet(this IServiceCollection services, Action<NacosAspNetOptions> optionsAccs)
        {
            services.Configure(optionsAccs);

            var options = new NacosAspNetOptions();
            optionsAccs.Invoke(options);
            services.AddNacosV2Naming(x => options.BuildSdkOptions());
            services.AddSingleton<RegSvcBgTask>();

            return services;
        }


        public static async Task<IApplicationBuilder> UseNacosAspNet(this IApplicationBuilder app, IHostApplicationLifetime lifetime)
        {
            RegSvcBgTask regSvcBgTask = app.ApplicationServices.GetRequiredService<RegSvcBgTask>();
            await regSvcBgTask.StartAsync();
            lifetime.ApplicationStopping.Register(async () => {
                await regSvcBgTask.StopAsync();
            });
            return app;
        }
    }
}
