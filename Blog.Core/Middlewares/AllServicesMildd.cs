using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Core.Extensions
{
    /// <summary>
    /// Cors 启动服务
    /// </summary>
    public static class AllServicesMildd
    {
        public static void UseAllServicesMildd(this IApplicationBuilder app, IServiceCollection _services, List<Type> tsDIAutofac)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            app.Map("/allservices", builder => builder.Run(async context =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.WriteAsync("<style> td{padding:8px;} tr:nth-child(2n){background: #f0f9eb;}</style>");

                await context.Response.WriteAsync($"<h3>所有服务{_services.Count}个</h3><table><thead><tr><th>类型</th><th>生命周期</th><th>Instance</th></tr></thead><tbody>");
                foreach (var item in tsDIAutofac.Where(s => !s.IsInterface))
                {
                    var interfaceType = item.GetInterfaces();
                    foreach (var typeArray in interfaceType)
                    {
                        await context.Response.WriteAsync("<tr>");
                        await context.Response.WriteAsync($"<td style='width:400px'>{typeArray?.Name}</td>");
                        await context.Response.WriteAsync($"<td style='width:100px;'>Scoped</td>");
                        await context.Response.WriteAsync($"<td style='width:400px'>{item?.Name}</td>");
                        await context.Response.WriteAsync("</tr>");
                    }
                }

                foreach (var svc in _services)
                {
                    await context.Response.WriteAsync("<tr>");
                    await context.Response.WriteAsync($"<td style='width:400px'>{svc.ServiceType.Name}</td>");
                    await context.Response.WriteAsync($"<td style='width:100px;'>{svc.Lifetime}</td>");
                    await context.Response.WriteAsync($"<td style='width:400px'>{svc.ImplementationType?.Name}</td>");
                    await context.Response.WriteAsync("</tr>");
                }
                await context.Response.WriteAsync("</tbody></table>");
            }));
        }
    }
}
