using Blog.Core.Common.LogHelper;
using Blog.Core.Hubs;
using Blog.Core.Log;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using StackExchange.Profiling;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Core.Filter
{
    /// <summary>
    /// 全局权限处理
    /// </summary>
    public class GlobalAuthorizeFilter : AuthorizeFilter
    {
        public GlobalAuthorizeFilter(string policy) : base(policy)
        {
        }

        public override Task OnAuthorizationAsync(Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext context)
        {
            // If there is another authorize filter, do nothing
            if (context.Filters.Any(item => item is IAsyncAuthorizationFilter && item != this))
            {
                return Task.FromResult(0);
            }


            if (context.ActionDescriptor.FilterDescriptors.Select(f => f.Filter).OfType<TypeFilterAttribute>().Any(f => f.ImplementationType.Equals(typeof(IgonreGlobalActionFilter))))
            {
                return Task.FromResult(0);
            }


            //Otherwise apply this policy
            return base.OnAuthorizationAsync(context);
        }



    }

    public class IgonreGlobalActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await context.HttpContext.Response.WriteAsync($"{GetType().Name} in. \r\n");

            await next();

            await context.HttpContext.Response.WriteAsync($"{GetType().Name} out. \r\n");
        }
    }



}
