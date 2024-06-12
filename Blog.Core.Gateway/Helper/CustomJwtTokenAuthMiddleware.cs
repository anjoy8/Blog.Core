using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authentication;
using Blog.Core.Common;
using Blog.Core.Common.Caches;
using Blog.Core.Common.Helper;

namespace Blog.Core.AuthHelper
{
    /// <summary>
    /// 中间件
    /// 原做为自定义授权中间件
    /// 先做检查 header token的使用
    /// </summary>
    public class CustomJwtTokenAuthMiddleware
    {
        private readonly ICaching _cache;
      
       
        /// <summary>
        /// 验证方案提供对象
        /// </summary>
        public IAuthenticationSchemeProvider Schemes { get; set; }

        /// <summary>
        /// 请求上下文
        /// </summary>
        private readonly RequestDelegate _next;
        

        public CustomJwtTokenAuthMiddleware(RequestDelegate next, IAuthenticationSchemeProvider schemes, AppSettings appset,ICaching cache)
        {
            _cache = cache;
            _next = next;
            Schemes = schemes;
        }

        /// <summary>
        /// 网关授权
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpContext)
        {
            var questUrl = httpContext?.Request.Path.Value.ToLower();
            if (string.IsNullOrEmpty(questUrl)) return;
            //白名单验证
            if (CheckWhiteList(questUrl))
            {
                await _next.Invoke(httpContext);
                return;
            }
            //黑名单验证
            if(CheckBlackList(questUrl))
            {
                return;
            }

            List<PermissionItem> Permissions= new();

            httpContext.Features.Set<IAuthenticationFeature>(new AuthenticationFeature
            {
                OriginalPath = httpContext.Request.Path,
                OriginalPathBase = httpContext.Request.PathBase
            });

            //判断请求是否拥有凭据，即有没有登录
            var defaultAuthenticate = await Schemes.GetDefaultAuthenticateSchemeAsync();
            if (defaultAuthenticate != null)
            {
                var Authresult = await httpContext.AuthenticateAsync(defaultAuthenticate.Name);
                if (Authresult?.Principal != null)
                {
                    httpContext.User = Authresult.Principal;
                    // 获取当前用户的角色信息
                    var currentUserRoles = (from item in httpContext.User.Claims
                                            where item.Type == "CofRole"
                                            select item.Value).ToList();
                    var isMatchRole = false;
                    var permisssionRoles = Permissions.Where(w => currentUserRoles.Contains(w.Role));
                    foreach (var item in permisssionRoles)
                    {
                        try
                        {
                            if (Regex.IsMatch(questUrl, item.Url, RegexOptions.IgnoreCase))
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
                    if (currentUserRoles.Count <= 0 || !isMatchRole)
                    {
                        await httpContext.Cof_SendResponse(HttpStatusCode.ServiceUnavailable, "未授权此资源");
                        return ;
                    }
                }
                else
                {
                    await httpContext.Cof_SendResponse(HttpStatusCode.Unauthorized, "请重新登录");
                    return ;
                }

            }
            else
            {
                await httpContext.Cof_SendResponse(HttpStatusCode.Unauthorized, "系统鉴权出错");
                return ;
            }
            await _next.Invoke(httpContext);
        }

        /// <summary>
        /// 返回相应
        /// </summary>
        /// <param name="context"></param>
        /// <param name="message"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private async Task SendResponse(HttpContext context, string message, HttpStatusCode code)
        {
            context.Response.StatusCode = (int)code;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(message);
        }
        
        /// <summary>
        /// 判断是否在白名单内，支持通配符 **** 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool CheckWhiteList(string url)
        {
            List<Urlobj> WhiteList = _cache.Cof_GetICaching<List<Urlobj>>("WhiteList", () => AppSettings.app<Urlobj>("WhiteList"), 10);

            if (!WhiteList.Cof_CheckAvailable()) return false;
            foreach (var Urlitem in WhiteList)
            {
                if (Urlitem.url.Equals(url, StringComparison.OrdinalIgnoreCase)) return true;

                if (Urlitem.url.IndexOf("****") > 0)
                {
                    string UrlitemP = Urlitem.url.Replace("****", "");
                    if (Regex.IsMatch(url, UrlitemP, RegexOptions.IgnoreCase)) return true;
                    if (url.Length >= UrlitemP.Length && UrlitemP.ToLower() == url.Substring(0, UrlitemP.Length).ToLower()) return true;

                }
            }
            return false;

        }

        public bool CheckBlackList(string url)
        {
            List<Urlobj> BlackList = _cache.Cof_GetICaching<List<Urlobj>>("BlackList", () => AppSettings.app<Urlobj>("BlackList"), 10);
            
            if (!BlackList.Cof_CheckAvailable()) return false;
            foreach (var Urlitem in BlackList)
            {
                if (Urlitem.url.Equals(url, StringComparison.OrdinalIgnoreCase)) return true;

                if (Urlitem.url.IndexOf("****") > 0)
                {
                    string UrlitemP = Urlitem.url.Replace("****", "");
                    if (Regex.IsMatch(url, UrlitemP, RegexOptions.IgnoreCase)) return true;
                    if (url.Length >= UrlitemP.Length && UrlitemP.ToLower() == url.Substring(0, UrlitemP.Length).ToLower()) return true;

                }
            }
            return false;

        }
    }

    public class Urlobj
    { 
        public string url { get; set; }
    }
}

