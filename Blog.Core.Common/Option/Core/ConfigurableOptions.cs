using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Blog.Core.Common.Option.Core;

public static class ConfigurableOptions
{
    /// <summary>添加选项配置</summary>
    /// <typeparam name="TOptions">选项类型</typeparam>
    /// <param name="services">服务集合</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddConfigurableOptions<TOptions>(this IServiceCollection services)
        where TOptions : class, IConfigurableOptions
    {
        Type optionsType = typeof(TOptions);
        string path = GetConfigurationPath(optionsType);
        services.Configure<TOptions>(App.Configuration.GetSection(path));

        return services;
    }

    public static IServiceCollection AddConfigurableOptions(this IServiceCollection services, Type type)
    {
        string path = GetConfigurationPath(type);
        var config = App.Configuration.GetSection(path);

        Type iOptionsChangeTokenSource = typeof(IOptionsChangeTokenSource<>);
        Type iConfigureOptions = typeof(IConfigureOptions<>);
        Type configurationChangeTokenSource = typeof(ConfigurationChangeTokenSource<>);
        Type namedConfigureFromConfigurationOptions = typeof(NamedConfigureFromConfigurationOptions<>);
        iOptionsChangeTokenSource = iOptionsChangeTokenSource.MakeGenericType(type);
        iConfigureOptions = iConfigureOptions.MakeGenericType(type);
        configurationChangeTokenSource = configurationChangeTokenSource.MakeGenericType(type);
        namedConfigureFromConfigurationOptions = namedConfigureFromConfigurationOptions.MakeGenericType(type);

        services.AddOptions();
        services.AddSingleton(iOptionsChangeTokenSource,
            Activator.CreateInstance(configurationChangeTokenSource, Options.DefaultName, config) ?? throw new InvalidOperationException());
        return services.AddSingleton(iConfigureOptions,
            Activator.CreateInstance(namedConfigureFromConfigurationOptions, Options.DefaultName, config) ?? throw new InvalidOperationException());
    }

    /// <summary>获取配置路径</summary>
    /// <param name="optionsType">选项类型</param>
    /// <returns></returns>
    public static string GetConfigurationPath(Type optionsType)
    {
        var endPath = new[] {"Option", "Options"};
        var configurationPath = optionsType.Name;
        foreach (var s in endPath)
        {
            if (configurationPath.EndsWith(s))
            {
                return configurationPath[..^s.Length];
            }
        }

        return configurationPath;
    }
}