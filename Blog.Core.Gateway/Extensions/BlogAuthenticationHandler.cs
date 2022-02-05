using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Blog.Core.AdminMvc
{
    public class BlogAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BlogAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new List<Claim>()
            {
                new Claim("gw", "gw")
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