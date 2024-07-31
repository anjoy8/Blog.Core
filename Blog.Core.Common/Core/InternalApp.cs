using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Blog.Core.Common.Core;

/// <summary>
/// 内部只用于初始化使用
/// </summary>
public static class InternalApp
{
    internal static IServiceCollection InternalServices;

    /// <summary>根服务</summary>
    internal static IServiceProvider RootServices;

    /// <summary>获取Web主机环境</summary>
    internal static IWebHostEnvironment WebHostEnvironment;

    /// <summary>获取泛型主机环境</summary>
    internal static IHostEnvironment HostEnvironment;

    /// <summary>配置对象</summary>
    internal static IConfiguration Configuration;

    public static void ConfigureApplication(this WebApplicationBuilder wab)
    {
        HostEnvironment = wab.Environment;
        WebHostEnvironment = wab.Environment;
        InternalServices = wab.Services;
    }

    public static void ConfigureApplication(this IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public static void ConfigureApplication(this IHost app)
    {
        RootServices = app.Services;
    }

    public static void ConfigureApplication(this IServiceCollection services)
    {
        InternalServices = services;
    }

    public static void ConfigureApplication(this IServiceProvider services)
    {
        RootServices = services;
    }
}