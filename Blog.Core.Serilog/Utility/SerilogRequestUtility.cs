using Blog.Core.Common.Extensions;
using Blog.Core.Common.Https;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;

namespace Blog.Core.Serilog.Utility;

public class SerilogRequestUtility
{
    public const string HttpMessageTemplate =
        "HTTP {RequestMethod} {RequestPath} QueryString:{QueryString} Body:{Body}  responded {StatusCode} in {Elapsed:0.0000} ms";

    private static readonly List<string> _ignoreUrl = new()
    {
        "/job",
    };

    private static LogEventLevel DefaultGetLevel(HttpContext ctx,
        double _,
        Exception? ex)
    {
        return ex is null && ctx.Response.StatusCode <= 499 ? LogEventLevel.Information : LogEventLevel.Error;
    }

    public static LogEventLevel GetRequestLevel(HttpContext ctx, double _, Exception? ex) =>
        ex is null && ctx.Response.StatusCode <= 499 ? IgnoreRequest(ctx) : LogEventLevel.Error;

    private static LogEventLevel IgnoreRequest(HttpContext ctx)
    {
        var path = ctx.Request.Path.Value;
        if (path.IsNullOrEmpty())
        {
            return LogEventLevel.Information;
        }

        return _ignoreUrl.Any(s => path.StartsWith(s)) ? LogEventLevel.Verbose : LogEventLevel.Information;
    }

    /// <summary>
    /// 从Request中增加附属属性
    /// </summary>
    /// <param name="diagnosticContext"></param>
    /// <param name="httpContext"></param>
    public static void EnrichFromRequest(IDiagnosticContext diagnosticContext, HttpContext httpContext)
    {
        var request = httpContext.Request;

        diagnosticContext.Set("RequestHost", request.Host);
        diagnosticContext.Set("RequestScheme", request.Scheme);
        diagnosticContext.Set("Protocol", request.Protocol);
        diagnosticContext.Set("RequestIp", httpContext.GetRequestIp());

        diagnosticContext.Set("QueryString", request.QueryString.HasValue ? request.QueryString.Value : string.Empty);
        diagnosticContext.Set("Body", request.ContentLength > 0 ? request.GetRequestBody() : string.Empty);

        diagnosticContext.Set("ContentType", httpContext.Response.ContentType);

        var endpoint = httpContext.GetEndpoint();
        if (endpoint != null)
        {
            diagnosticContext.Set("EndpointName", endpoint.DisplayName);
        }
    }
}