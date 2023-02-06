using Microsoft.Extensions.Hosting;
using System;

namespace Blog.Core.Common.Core;

public static class InternalApp
{
    /// <summary>根服务</summary>
    public static IServiceProvider RootServices;

    public static void ConfigureApplication(this IHost app)
    {
        RootServices = app.Services;
    }
}