using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Blog.Core.Gateway.Extensions
{
    public class CustomAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public CustomAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // 可以查询数据库等操作
            // 获取当前用户不能放到token中的私密信息
            var userPhone = "15010000000";

            var claims = new List<Claim>()
            {
                new Claim("user-phone", userPhone),
                new Claim("gw-sign", "gw")
            };

            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, Scheme.Name));
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            await Task.CompletedTask;
            return AuthenticateResult.Success(ticket);
        }

        protected virtual string GetTokenStringFromHeader()
        {
            var token = string.Empty;
            string authorization = Request.Headers[HeaderNames.Authorization];

            if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith($"Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                token = authorization["Bearer ".Length..].Trim();
            }

            return token;
        }
    }
}