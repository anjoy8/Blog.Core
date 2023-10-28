using System.IO;
using Blog.Core.Common.Https;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;

namespace Blog.Core.Extensions.Middlewares;

public static class FluentResponseBodyMiddleware
{
	public static IApplicationBuilder UseResponseBodyRead(this IApplicationBuilder app)
	{
		return app.Use(async (context, next) =>
		{
			await using var swapStream = new FluentHttpResponseStream(context!.Features!.Get<IHttpResponseBodyFeature>()!,
				context!.Features!.Get<IHttpBodyControlFeature>()!);
			context.Response.Body = swapStream;
			await next(context);
			context.Response.Body.Seek(0, SeekOrigin.Begin);
		});
	}
}