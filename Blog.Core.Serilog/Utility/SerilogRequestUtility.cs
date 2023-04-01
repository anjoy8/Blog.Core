using Microsoft.AspNetCore.Http;
using Serilog.Events;

namespace Blog.Core.Serilog.Utility;

public class SerilogRequestUtility
{
    private static readonly List<string> _ignoreUrl = new()
    {
        "/job",
    };

    private static LogEventLevel DefaultGetLevel(
        HttpContext ctx,
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
}