using Blog.Core.Common.Core;
using Blog.Core.Common.HttpContextUser;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Blog.Core.Common;

public class App
{
    public static IServiceProvider RootServices => InternalApp.RootServices ;

    /// <summary>
    /// 获取请求上下文
    /// </summary>
    public static HttpContext HttpContext => RootServices?.GetService<IHttpContextAccessor>()?.HttpContext;

    public static IUser User => HttpContext == null ? null : RootServices?.GetService<IUser>();
}