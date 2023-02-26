using Blog.Core.AuthHelper;
using Blog.Core.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Blog.Core.Extensions
{
    /// <summary>
    /// Ids4权限 认证服务
    /// </summary>
    public static class Authentication_Ids4Setup
    {
        public static void AddAuthentication_Ids4Setup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));


            // 添加Identityserver4认证
            services.AddAuthentication(o =>
            {
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = nameof(ApiResponseHandler);
                o.DefaultForbidScheme = nameof(ApiResponseHandler);
            })
            .AddJwtBearer(options =>
            {
                options.Authority = AppSettings.app(new string[] { "Startup", "IdentityServer4", "AuthorizationUrl" });
                options.RequireHttpsMetadata = false;
                options.Audience = AppSettings.app(new string[] { "Startup", "IdentityServer4", "ApiName" });
            })
            .AddScheme<AuthenticationSchemeOptions, ApiResponseHandler>(nameof(ApiResponseHandler), o => { });
        }
    }
}
