using Blog.Core.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Nacos;
using Ocelot.Provider.Polly;
using System;
using System.Threading.Tasks;

namespace Blog.Core.Gateway.Extensions
{
    public static class CustomOcelotSetup
    {
        public static void AddCustomOcelotSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddAuthentication_JWTSetup();
            services.AddOcelot()
                .AddDelegatingHandler<CustomResultHandler>()
                //.AddNacosDiscovery()
                //.AddConsul()
                .AddPolly();
        }

        public static async Task<IApplicationBuilder> UseCustomOcelotMildd(this IApplicationBuilder app)
        {
            await app.UseOcelot();
            return app;
        }

    }
}
