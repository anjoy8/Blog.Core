using Blog.Core.Common.Core;
using Blog.Core.Common.Extensions;
using Blog.Core.Common.HttpContextUser;
using Blog.Core.Common.Option.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Blog.Core.Common;

public class App
{
    static App()
    {
        EffectiveTypes = Assemblies.SelectMany(GetTypes);
    }

    private static bool _isRun;

    /// <summary>是否正在运行</summary>
    public static bool IsBuild { get; set; }

    public static bool IsRun
    {
        get => _isRun;
        set => _isRun = IsBuild = value;
    }

    /// <summary>应用有效程序集</summary>
    public static readonly IEnumerable<Assembly> Assemblies = RuntimeExtension.GetAllAssemblies();

    /// <summary>有效程序集类型</summary>
    public static readonly IEnumerable<Type> EffectiveTypes;

    public static IServiceProvider RootServices => IsRun || IsBuild ? InternalApp.RootServices : null;

    /// <summary>获取Web主机环境，如，是否是开发环境，生产环境等</summary>
    public static IWebHostEnvironment WebHostEnvironment => InternalApp.WebHostEnvironment;

    /// <summary>获取泛型主机环境，如，是否是开发环境，生产环境等</summary>
    public static IHostEnvironment HostEnvironment => InternalApp.HostEnvironment;

    /// <summary>全局配置选项</summary>
    public static IConfiguration Configuration => InternalApp.Configuration;

    /// <summary>
    /// 获取请求上下文
    /// </summary>
    public static HttpContext HttpContext => RootServices?.GetService<IHttpContextAccessor>()?.HttpContext;

    public static IUser User => HttpContext == null ? null : RootServices?.GetService<IUser>();

    #region Service

    /// <summary>解析服务提供器</summary>
    /// <param name="serviceType"></param>
    /// <returns></returns>
    public static IServiceProvider GetServiceProvider(Type serviceType, bool mustBuild = false)
    {
        if (App.HostEnvironment == null || App.RootServices != null &&
            InternalApp.InternalServices
                .Where((u =>
                    u.ServiceType ==
                    (serviceType.IsGenericType ? serviceType.GetGenericTypeDefinition() : serviceType)))
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


    public static TService GetService<TService>(bool mustBuild = true) where TService : class =>
        App.GetService(typeof(TService), null, mustBuild) as TService;

    /// <summary>获取请求生存周期的服务</summary>
    /// <typeparam name="TService"></typeparam>
    /// <param name="serviceProvider"></param>
    /// <param name="mustBuild"></param>
    /// <returns></returns>
    public static TService GetService<TService>(IServiceProvider serviceProvider, bool mustBuild = true)
        where TService : class => App.GetService(typeof(TService), serviceProvider, mustBuild) as TService;

    /// <summary>获取请求生存周期的服务</summary>
    /// <param name="type"></param>
    /// <param name="serviceProvider"></param>
    /// <param name="mustBuild"></param>
    /// <returns></returns>
    public static object GetService(Type type, IServiceProvider serviceProvider = null, bool mustBuild = true) =>
        (serviceProvider ?? App.GetServiceProvider(type, mustBuild)).GetService(type);

    #endregion

    #region private

    /// <summary>加载程序集中的所有类型</summary>
    /// <param name="ass"></param>
    /// <returns></returns>
    private static IEnumerable<Type> GetTypes(Assembly ass)
    {
        Type[] source = Array.Empty<Type>();
        try
        {
            source = ass.GetTypes();
        }
        catch
        {
            $@"Error load `{ass.FullName}` assembly.".WriteErrorLine();
        }

        return source.Where(u => u.IsPublic);
    }

    #endregion

    #region Options

    /// <summary>获取配置</summary>
    /// <typeparam name="TOptions">强类型选项类</typeparam>
    /// <returns>TOptions</returns>
    public static TOptions GetConfig<TOptions>()
        where TOptions : class, IConfigurableOptions
    {
        TOptions instance = App.Configuration
            .GetSection(ConfigurableOptions.GetConfigurationPath(typeof(TOptions)))
            .Get<TOptions>();
        return instance;
    }

    /// <summary>获取选项</summary>
    /// <typeparam name="TOptions">强类型选项类</typeparam>
    /// <param name="serviceProvider"></param>
    /// <returns>TOptions</returns>
    public static TOptions GetOptions<TOptions>(IServiceProvider serviceProvider = null) where TOptions : class, new()
    {
        IOptions<TOptions> service = App.GetService<IOptions<TOptions>>(serviceProvider ?? App.RootServices, false);
        return service?.Value;
    }

    /// <summary>获取选项</summary>
    /// <typeparam name="TOptions">强类型选项类</typeparam>
    /// <param name="serviceProvider"></param>
    /// <returns>TOptions</returns>
    public static TOptions GetOptionsMonitor<TOptions>(IServiceProvider serviceProvider = null)
        where TOptions : class, new()
    {
        IOptionsMonitor<TOptions> service =
            App.GetService<IOptionsMonitor<TOptions>>(serviceProvider ?? App.RootServices, false);
        return service?.CurrentValue;
    }

    /// <summary>获取选项</summary>
    /// <typeparam name="TOptions">强类型选项类</typeparam>
    /// <param name="serviceProvider"></param>
    /// <returns>TOptions</returns>
    public static TOptions GetOptionsSnapshot<TOptions>(IServiceProvider serviceProvider = null)
        where TOptions : class, new()
    {
        IOptionsSnapshot<TOptions> service = App.GetService<IOptionsSnapshot<TOptions>>(serviceProvider, false);
        return service?.Value;
    }

    #endregion
}