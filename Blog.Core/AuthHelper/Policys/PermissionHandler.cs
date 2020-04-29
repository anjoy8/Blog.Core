using Blog.Core.Common.Helper;
using Blog.Core.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        private readonly IRoleModulePermissionServices _roleModulePermissionServices;
        private readonly IHttpContextAccessor _accessor;

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
            _roleModulePermissionServices = roleModulePermissionServices;
        }

        // 重写异步处理程序
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var httpContext = _accessor.HttpContext;

            if (!requirement.Permissions.Any())
            {
                var data = await _roleModulePermissionServices.RoleModuleMaps();
                var list = new List<PermissionItem>();
                // ids4和jwt切换
                // ids4
                if (Permissions.IsUseIds4)
                {
                    list = (from item in data
                            where item.IsDeleted == false
                            orderby item.Id
                            select new PermissionItem
                            {
                                Url = item.Module?.LinkUrl,
                                Role = item.Role?.Id.ObjToString(),
                            }).ToList();
                }
                // jwt
                else
                {
                    list = (from item in data
                            where item.IsDeleted == false
                            orderby item.Id
                            select new PermissionItem
                            {
                                Url = item.Module?.LinkUrl,
                                Role = item.Role?.Name.ObjToString(),
                            }).ToList();
                }
                requirement.Permissions = list;
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
                        // 将最新的角色和接口列表更新

                        // 这里暂时把代码移动到了Login获取token的api里,这样就不用每次都请求数据库,造成压力.
                        // 但是这样有个问题,就是如果修改了某一个角色的菜单权限,不会立刻更新,
                        // 需要让用户退出重新登录,如果你想实时更新,请把下边的注释打开即可.

                        //var data = await _roleModulePermissionServices.RoleModuleMaps();
                        //var list = (from item in data
                        //            where item.IsDeleted == false
                        //            orderby item.Id
                        //            select new PermissionItem
                        //            {
                        //                Url = item.Module?.LinkUrl,
                        //                Role = item.Role?.Name,
                        //            }).ToList();
                        //requirement.Permissions = list;

                        httpContext.User = result.Principal;

                        //权限中是否存在请求的url
                        //if (requirement.Permissions.GroupBy(g => g.Url).Where(w => w.Key?.ToLower() == questUrl).Count() > 0)
                        //if (isMatchUrl)
                        if (true)
                        {
                            // 获取当前用户的角色信息

                            var currentUserRoles = new List<string>();
                            // ids4和jwt切换
                            // ids4
                            if (Permissions.IsUseIds4)
                            {
                                currentUserRoles = (from item in httpContext.User.Claims
                                                    where item.Type == "role"
                                                    select item.Value).ToList();
                            }
                            else
                            {
                                // jwt
                                currentUserRoles = (from item in httpContext.User.Claims
                                                    where item.Type == requirement.ClaimType
                                                    select item.Value).ToList();
                            }

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

                        var isExp = false;
                        // ids4和jwt切换
                        // ids4
                        if (Permissions.IsUseIds4)
                        {
                            isExp = (httpContext.User.Claims.SingleOrDefault(s => s.Type == "exp")?.Value) != null && DateHelper.StampToDateTime(httpContext.User.Claims.SingleOrDefault(s => s.Type == "exp")?.Value) >= DateTime.Now;
                        }
                        else
                        {
                            // jwt
                            isExp = (httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration)?.Value) != null && DateTime.Parse(httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration)?.Value) >= DateTime.Now;
                        }
                        if (isExp)
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
                if (!(questUrl.Equals(requirement.LoginPath.ToLower(), StringComparison.Ordinal) && (!httpContext.Request.Method.Equals("POST") || !httpContext.Request.HasFormContentType)))
                {
                    context.Fail();
                    return;
                }
            }

            //context.Succeed(requirement);
        }
    }
}
