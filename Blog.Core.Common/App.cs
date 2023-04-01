using Blog.Core.Common.Core;
using Blog.Core.Common.HttpContextUser;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace Blog.Core.Common;

public class App
{
    public static IServiceProvider RootServices => InternalApp.RootServices;

    /// <summary>获取Web主机环境，如，是否是开发环境，生产环境等</summary>
    public static IWebHostEnvironment WebHostEnvironment => InternalApp.WebHostEnvironment;

    /// <summary>获取泛型主机环境，如，是否是开发环境，生产环境等</summary>
    public static IHostEnvironment HostEnvironment => InternalApp.HostEnvironment;

    /// <summary>
    /// 获取请求上下文
    /// </summary>
    public static HttpContext HttpContext => RootServices?.GetService<IHttpContextAccessor>()?.HttpContext;

    public static IUser User => HttpContext == null ? null : RootServices?.GetService<IUser>();

    /// <summary>解析服务提供器</summary>
    /// <param name="serviceType"></param>
    /// <returns></returns>
    public static IServiceProvider GetServiceProvider(Type serviceType, bool mustBuild = false)
    {
        if (App.HostEnvironment == null || App.RootServices != null &&
            InternalApp.InternalServices
                .Where((u => u.ServiceType == (serviceType.IsGenericType ? serviceType.GetGenericTypeDefinition() : serviceType)))
                .Any((u => u.Lifetime == ServiceLifetime.Singleton)))
            return App.RootServices;
        HttpContext httpContext = App.HttpContext;
        if (httpContext?.RequestServices != null)
            return httpContext.RequestServices;
        if (App.RootServices != null)
        {
            IServiceScope scope = App.RootServices.CreateScope();
            return scope.ServiceProvider;
        }

        if (mustBuild)
        {
            throw new ApplicationException("当前不可用，必须要等到 WebApplication Build后");
        }

        ServiceProvider serviceProvider = InternalApp.InternalServices.BuildServiceProvider();
        return serviceProvider;
    }


    public static TService GetService<TService>(bool mustBuild = true) where TService : class => App.GetService(typeof(TService), null, mustBuild) as TService;

    /// <summary>获取请求生存周期的服务</summary>
    /// <typeparam name="TService"></typeparam>
    /// <param name="serviceProvider"></param>
    /// <param name="mustBuild"></param>
    /// <returns></returns>
    public static TService GetService<TService>(IServiceProvider serviceProvider, bool mustBuild = true) where TService : class => App.GetService(typeof(TService), serviceProvider, mustBuild) as TService;

    /// <summary>获取请求生存周期的服务</summary>
    /// <param name="type"></param>
    /// <param name="serviceProvider"></param>
    /// <param name="mustBuild"></param>
    /// <returns></returns>
    public static object GetService(Type type, IServiceProvider serviceProvider = null, bool mustBuild = true) => (serviceProvider ?? App.GetServiceProvider(type, mustBuild)).GetService(type);

    public static TOptions GetOptions<TOptions>(IServiceProvider serviceProvider = null) where TOptions : class, new()
    {
        IOptions<TOptions> service = App.GetService<IOptions<TOptions>>(serviceProvider ?? App.RootServices, false);
        return service?.Value;
    }
}