using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Blog.Core.Common.Core;

public static class InternalApp
{
    public static IServiceCollection InternalServices;

    /// <summary>根服务</summary>
    public static IServiceProvider RootServices;

    /// <summary>获取Web主机环境</summary>
    public static IWebHostEnvironment WebHostEnvironment;

    /// <summary>获取泛型主机环境</summary>
    public static IHostEnvironment HostEnvironment;

    public static void ConfigureApplication(this WebApplicationBuilder wab)
    {
        HostEnvironment = wab.Environment;
        WebHostEnvironment = wab.Environment;
        InternalServices = wab.Services;
    }


    public static void ConfigureApplication(this IHost app)
    {
        RootServices = app.Services;
    }
}