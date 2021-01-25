using Microsoft.AspNetCore.Builder;
using Ocelot.Middleware;
using System.Threading.Tasks;

namespace Blog.Core.Gateway.Extensions
{
    public static class OcelotMildd
    {
        public static async Task<IApplicationBuilder> UseOcelotMildd(this IApplicationBuilder app)
        {
            await app.UseOcelot();
            return app;
        }

    }
}
