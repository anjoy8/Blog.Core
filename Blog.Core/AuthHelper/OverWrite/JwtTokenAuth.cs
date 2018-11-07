using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog.Core.AuthHelper
{
    /// <summary>
    /// 
    /// </summary>
    public class JwtTokenAuth
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly RequestDelegate _next;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public JwtTokenAuth(RequestDelegate next)
        {
            _next = next;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public Task Invoke(HttpContext httpContext)
        {
            //检测是否包含'Authorization'请求头
            if (!httpContext.Request.Headers.ContainsKey("Authorization"))
            {
                return _next(httpContext);
            }
            //var tokenHeader = httpContext.Request.Headers["Authorization"].ToString();
            var tokenHeader = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            TokenModelJWT tm = JwtHelper.SerializeJWT(tokenHeader);

            //授权
            var claimList = new List<Claim>();
            var claim = new Claim(ClaimTypes.Role, tm.Role);
            claimList.Add(claim);
            var identity = new ClaimsIdentity(claimList);
            var principal = new ClaimsPrincipal(identity);
            httpContext.User = principal;

            return _next(httpContext);
        }

    }
}

