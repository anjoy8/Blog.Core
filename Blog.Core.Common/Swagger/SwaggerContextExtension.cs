using Blog.Core.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace Blog.Core.Common.Swagger;

public static class SwaggerContextExtension
{
	public const string SwaggerCodeKey = "swagger-code";
	public const string SwaggerJwt = "swagger-jwt";

	public static bool IsSuccessSwagger()
	{
		return App.HttpContext?.GetSession()?.GetString(SwaggerCodeKey) == "success";
	}

	public static bool IsSuccessSwagger(this HttpContext context)
	{
		return context.GetSession()?.GetString(SwaggerCodeKey) == "success";
	}

	public static void SuccessSwagger()
	{
		App.HttpContext?.GetSession()?.SetString(SwaggerCodeKey, "success");
	}

	public static void SuccessSwagger(this HttpContext context)
	{
		context.GetSession()?.SetString(SwaggerCodeKey, "success");
	}

	public static void SuccessSwaggerJwt(this HttpContext context, string token)
	{
		context.GetSession()?.SetString(SwaggerJwt, token);
	}

	public static string GetSuccessSwaggerJwt(this HttpContext context)
	{
		return context.GetSession()?.GetString(SwaggerJwt);
	}


	public static void RedirectSwaggerLogin(this HttpContext context)
	{
		var returnUrl = context.Request.GetDisplayUrl(); //获取当前url地址 
		context.Response.Redirect("/swg-login.html?returnUrl=" + returnUrl);
	}
}