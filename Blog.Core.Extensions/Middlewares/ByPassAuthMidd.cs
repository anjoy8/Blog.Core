using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog.Core.Middlewares
{
    /// <summary>
    /// 测试用户，用来通过鉴权
    /// JWT：?userid=8&rolename=AdminTest
    /// </summary>
    public class ByPassAuthMidd
    {
        private readonly RequestDelegate _next;
        // 定义变量：当前用户Id，会常驻内存。
        private string _currentUserId;
        // 同理定义：当前角色名
        private string _currentRoleName;
        public ByPassAuthMidd(RequestDelegate next)
        {
            _next = next;
            _currentUserId = null;
            _currentRoleName = null;
        }


        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path;
            // 请求地址，通过Url参数的形式，设置用户id和rolename
            if (path == "/noauth")
            {
                var userid = context.Request.Query["userid"];
                if (!string.IsNullOrEmpty(userid))
                {
                    _currentUserId = userid;
                }

                var rolename = context.Request.Query["rolename"];
                if (!string.IsNullOrEmpty(rolename))
                {
                    _currentRoleName = rolename;
                }

                await SendOkResponse(context, $"User set to {_currentUserId} and Role set to {_currentRoleName}.");
            }
            // 重置角色信息
            else if (path == "/noauth/reset")
            {
                _currentUserId = null;
                _currentRoleName = null;

                await SendOkResponse(context, $"User set to none. Token required for protected endpoints.");
            }
            else
            {
                var currentUserId = _currentUserId;
                var currentRoleName = _currentRoleName;

                // 你也可以通过Header的形式。

                //var authHeader = context.Request.Headers["Authorization"];
                //if (authHeader != StringValues.Empty)
                //{
                //    var header = authHeader.FirstOrDefault();
                //    if (!string.IsNullOrEmpty(header) && header.StartsWith("User ") && header.Length > "User ".Length)
                //    {
                //        currentUserId = header.Substring("User ".Length);
                //    }
                //}

                // 如果用户id和rolename都不为空
                // 可以配置HttpContext.User信息了，也就相当于登录了。
                if (!string.IsNullOrEmpty(currentUserId) && !string.IsNullOrEmpty(currentRoleName))
                {
                    var user = new ClaimsIdentity(new[] {
                    // 用户id   
                    new Claim("sub", currentUserId),

                    // 用户名、角色名
                    new Claim("name", "Test user"),
                    new Claim(ClaimTypes.Name, "Test user"),
                    new Claim("role", currentRoleName),
                    new Claim(ClaimTypes.Role, currentRoleName),

                    // 过期时间，两个：jwt/ids4
                    new Claim ("exp",$"{new DateTimeOffset(DateTime.Now.AddDays(10100)).ToUnixTimeSeconds()}"),
                    new Claim(ClaimTypes.Expiration, DateTime.Now.AddDays(1).ToString()),

                    // 其他参数
                    new Claim("nonce", Guid.NewGuid().ToString()),
                    new Claim("http://schemas.microsoft.com/identity/claims/identityprovider", "ByPassAuthMiddleware"),
                    new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname","User"),
                    new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname","Microsoft")}
                    , "ByPassAuth");

                    context.User = new ClaimsPrincipal(user);
                }

                await _next.Invoke(context);
            }
        }

        /// <summary>
        /// 返回相应
        /// </summary>
        /// <param name="context"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task SendOkResponse(HttpContext context, string message)
        {
            context.Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(message);
        }
    }
}

