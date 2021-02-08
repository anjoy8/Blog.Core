using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
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

            var basePath = AppContext.BaseDirectory;

            services.AddOcelot().AddConsul().AddPolly();
        }

        public static async Task<IApplicationBuilder> UseCustomOcelotMildd(this IApplicationBuilder app)
        {
            await app.UseOcelot();
            return app;
        }

    }
}
