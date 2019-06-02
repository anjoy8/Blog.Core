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

        /// <summary>
        /// services 层注入
        /// </summary>
        public IRoleModulePermissionServices roleModulePermissionServices { get; set; }

        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="schemes"></param>
        /// <param name="roleModulePermissionServices"></param>
        public PermissionHandler(IAuthenticationSchemeProvider schemes, IRoleModulePermissionServices roleModulePermissionServices)
        {
            Schemes = schemes;
            this.roleModulePermissionServices = roleModulePermissionServices;
        }

        // 重载异步处理程序
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            // 将最新的角色和接口列表更新
            var data = await roleModulePermissionServices.GetRoleModule();
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
                        //context.Fail();
                        //return;


                        //自定义返回数据
                        var payload = JsonConvert.SerializeObject(new { Code = "401", Message = "很抱歉，您无权访问该接口!" });
                        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        filterContext.Result = new JsonResult(payload);
                        context.Succeed(requirement);
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

                        // 取消对URL的判断，因为只需判断该角色下是否匹配当前URL即可，若不匹配都是无效请求
                        //var isMatchUrl = false;
                        //var permisssionGroup = requirement.Permissions.GroupBy(g => g.Url);
                        //foreach (var item in permisssionGroup)
                        //{
                        //    try
                        //    {
                        //        if (Regex.Match(questUrl, item.Key?.ObjToString().ToLower())?.Value == questUrl)
                        //        {
                        //            isMatchUrl = true;
                        //            break;
                        //        }
                        //    }
                        //    catch (Exception)
                        //    {
                        //    }
                        //}

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

                                //context.Fail();
                                //return;


                                //自定义返回数据
                                var payload = JsonConvert.SerializeObject(new { Code = "403", Message = "很抱歉，您无权访问该接口!" });
                                httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                                filterContext.Result = new JsonResult(payload);
                                context.Succeed(requirement);
                                return;


                                // 可以在这里设置跳转页面，不过还是会访问当前接口地址的
                                //httpContext.Response.Redirect(requirement.DeniedAction);
                            }
                        }
                        //else
                        //{
                        //    context.Fail();
                        //    return;

                        //}
                        //判断过期时间（这里仅仅是最坏验证原则，你可以不要这个if else的判断，因为我们使用的官方验证，Token过期后上边的result?.Principal 就为 null 了，进不到这里了，因此这里其实可以不用验证过期时间，只是做最后严谨判断）
                        if ((httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration)?.Value) != null && DateTime.Parse(httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration)?.Value) >= DateTime.Now)
                        {
                            context.Succeed(requirement);
                        }
                        else
                        {
                            //context.Fail();
                            //return;


                            //自定义返回数据
                            var payload = JsonConvert.SerializeObject(new { Code = "401", Message = "很抱歉，您无权访问该接口!" });
                            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            filterContext.Result = new JsonResult(payload);
                            context.Succeed(requirement);
                            return;
                        }
                        return;
                    }
                }
                //判断没有登录时，是否访问登录的url,并且是Post请求，并且是form表单提交类型，否则为失败
                if (!questUrl.Equals(requirement.LoginPath.ToLower(), StringComparison.Ordinal) && (!httpContext.Request.Method.Equals("POST")
                                                                                                    || !httpContext.Request.HasFormContentType))
                {
                    //context.Fail();
                    //return;


                    //自定义返回数据
                    var payload = JsonConvert.SerializeObject(new { Code = "401", Message = "很抱歉，您无权访问该接口!" });
                    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    filterContext.Result = new JsonResult(payload);
                }
            }

            context.Succeed(requirement);
        }
    }
}
