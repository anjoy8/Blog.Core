using Blog.Core.Common;
using Blog.Core.Common.Option.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Blog.Core.Extensions.ServiceExtensions;

public static class AllOptionRegister
{
    public static void AddAllOptionRegister(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        foreach (var optionType in App.EffectiveTypes.Where(s =>
                     !s.IsInterface && typeof(IConfigurableOptions).IsAssignableFrom(s)))
        {
            services.AddConfigurableOptions(optionType);
        }
    }
}