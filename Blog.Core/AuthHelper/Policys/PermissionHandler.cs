using Blog.Core.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Blog.Core.AuthHelper
{
    /// <summary>
    /// 权限授权处理器
    /// </summary>
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        /// <summary>
        /// 验证方案提供对象
        /// </summary>
        public IAuthenticationSchemeProvider Schemes { get; set; }
        private readonly IHttpContextAccessor _accessor;


        /// <summary>
        /// services 层注入
        /// </summary>
        public IRoleModulePermissionServices RoleModulePermissionServices { get; set; }

        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="schemes"></param>
        /// <param name="roleModulePermissionServices"></param>
        /// <param name="accessor"></param>
        public PermissionHandler(IAuthenticationSchemeProvider schemes, IRoleModulePermissionServices roleModulePermissionServices, IHttpContextAccessor accessor)
        {
            _accessor = accessor;
            Schemes = schemes;
            this.RoleModulePermissionServices = roleModulePermissionServices;
        }

        // 重写异步处理程序
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            /*
             * 
             * 首先必须在 controller 上进行配置 Authorize ，可以策略授权，也可以角色等基本授权
             * 
             * 1、开启公约， startup 中的全局授权过滤公约：o.Conventions.Insert(0, new GlobalRouteAuthorizeConvention());
             * 
             * 2、不开启公约，使用 IHttpContextAccessor ，也能实现效果；
             */

            // 将最新的角色和接口列表更新
            var data = await RoleModulePermissionServices.RoleModuleMaps();
            var list = (from item in data
                        where item.IsDeleted == false
                        orderby item.Id
                        select new PermissionItem
                        {
                            Url = item.Module?.LinkUrl,
                            Role = item.Role?.Name,
                        }).ToList();

            requirement.Permissions = list;


            //从AuthorizationHandlerContext转成HttpContext，以便取出表求信息
            var filterContext = (context.Resource as Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext);
            var httpContext = (context.Resource as Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext)?.HttpContext;

            if (httpContext == null)
            {
                httpContext = _accessor.HttpContext;
            }

            //请求Url
            if (httpContext != null)
            {
                var questUrl = httpContext.Request.Path.Value.ToLower();
                //判断请求是否停止
                var handlers = httpContext.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
                foreach (var scheme in await Schemes.GetRequestHandlerSchemesAsync())
                {
                    if (await handlers.GetHandlerAsync(httpContext, scheme.Name) is IAuthenticationRequestHandler handler && await handler.HandleRequestAsync())
                    {
                        context.Fail();
                        return;
                    }
                }
                //判断请求是否拥有凭据，即有没有登录
                var defaultAuthenticate = await Schemes.GetDefaultAuthenticateSchemeAsync();
                if (defaultAuthenticate != null)
                {
                    var result = await httpContext.AuthenticateAsync(defaultAuthenticate.Name);
                    //result?.Principal不为空即登录成功
                    if (result?.Principal != null)
                    {
                        httpContext.User = result.Principal;

                        //权限中是否存在请求的url
                        //if (requirement.Permissions.GroupBy(g => g.Url).Where(w => w.Key?.ToLower() == questUrl).Count() > 0)
                        //if (isMatchUrl)
                        if (true)
                        {
                            // 获取当前用户的角色信息
                            var currentUserRoles = (from item in httpContext.User.Claims
                                                    where item.Type == requirement.ClaimType
                                                    select item.Value).ToList();

                            var isMatchRole = false;
                            var permisssionRoles = requirement.Permissions.Where(w => currentUserRoles.Contains(w.Role));
                            foreach (var item in permisssionRoles)
                            {
                                try
                                {
                                    if (Regex.Match(questUrl, item.Url?.ObjToString().ToLower())?.Value == questUrl)
                                    {
                                        isMatchRole = true;
                                        break;
                                    }
                                }
                                catch (Exception)
                                {
                                    // ignored
                                }
                            }

                            //验证权限
                            //if (currentUserRoles.Count <= 0 || requirement.Permissions.Where(w => currentUserRoles.Contains(w.Role) && w.Url.ToLower() == questUrl).Count() <= 0)
                            if (currentUserRoles.Count <= 0 || !isMatchRole)
                            {
                                context.Fail();
                                return;
                            }
                        }
                       
                        //判断过期时间（这里仅仅是最坏验证原则，你可以不要这个if else的判断，因为我们使用的官方验证，Token过期后上边的result?.Principal 就为 null 了，进不到这里了，因此这里其实可以不用验证过期时间，只是做最后严谨判断）
                        if ((httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration)?.Value) != null && DateTime.Parse(httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration)?.Value) >= DateTime.Now)
                        {
                            context.Succeed(requirement);
                        }
                        else
                        {
                            context.Fail();
                            return;
                        }
                        return;
                    }
                }
                //判断没有登录时，是否访问登录的url,并且是Post请求，并且是form表单提交类型，否则为失败
                if (!questUrl.Equals(requirement.LoginPath.ToLower(), StringComparison.Ordinal) && (!httpContext.Request.Method.Equals("POST") || !httpContext.Request.HasFormContentType))
                {
                    context.Fail();
                    return;
                }
            }

            context.Succeed(requirement);
        }
    }
}
