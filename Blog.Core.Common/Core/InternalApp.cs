using Microsoft.AspNetCore.Builder;
using System;

namespace Blog.Core.Common.Core;

public static class InternalApp
{
    /// <summary>根服务</summary>
    public static IServiceProvider RootServices;

    public static void ConfigureApplication(this WebApplication app)
    {
        app.Lifetime.ApplicationStarted.Register(() => { InternalApp.RootServices = app.Services; });

        app.Lifetime.ApplicationStopped.Register(() => { InternalApp.RootServices = null; });
    }
}