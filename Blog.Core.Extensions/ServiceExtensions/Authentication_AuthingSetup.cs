using Blog.Core.AuthHelper;
using Blog.Core.Common;
using Blog.Core.Common.HttpContextUser;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.JwtExtensions;
using System;

namespace Blog.Core.Extensions
{
    /// <summary>
    /// Authing权限 认证服务
    /// </summary>
    public static class Authentication_AuthingSetup
    {
        public static void AddAuthentication_AuthingSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = AppSettings.app(new string[] { "Startup", "Authing", "Issuer" }),
                ValidAudience = AppSettings.app(new string[] { "Startup", "Authing", "Audience" }),
                ValidAlgorithms = new string[] { "RS256" },
                //ValidateLifetime = true,
                //ClockSkew = TimeSpan.FromSeconds(30),
                //RequireExpirationTime = true,
            };

            services.AddAuthentication(o =>
            {
                //认证middleware配置
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = nameof(ApiResponseHandler);
                o.DefaultForbidScheme = nameof(ApiResponseHandler);
            })
            .AddJwtBearer(o =>
            {
                //主要是jwt  token参数设置
                o.TokenValidationParameters = tokenValidationParameters;
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.IncludeErrorDetails = true;
                o.SetJwksOptions(new JwkOptions(AppSettings.app(new string[] { "Startup", "Authing", "JwksUri" }), AppSettings.app(new string[] { "Startup", "Authing", "Issuer" }), new TimeSpan(TimeSpan.TicksPerDay)));
            })
            .AddScheme<AuthenticationSchemeOptions, ApiResponseHandler>(nameof(ApiResponseHandler), o => { });

        }
    }
}
