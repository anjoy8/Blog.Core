using Blog.Core.Common;
using Blog.Core.Common.LogHelper;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using SqlSugar;

namespace Blog.Core.Serilog.Extensions;

public static class LoggerConfigurationExtensions
{
    public static LoggerConfiguration WriteToConsole(this LoggerConfiguration loggerConfiguration)
    {
        //输出普通日志
        loggerConfiguration = loggerConfiguration.WriteTo.Logger(lg =>
            lg.FilterRemoveSqlLog().WriteTo.Console());

        //输出SQL
        loggerConfiguration = loggerConfiguration.WriteTo.Logger(lg =>
            lg.FilterSqlLog().Filter.ByIncludingOnly(Matching.WithProperty<bool>(LogContextStatic.SqlOutToConsole, s => s))
                .WriteTo.Console());

        return loggerConfiguration;
    }

    public static LoggerConfiguration WriteToFile(this LoggerConfiguration loggerConfiguration)
    {
        //输出SQL
        loggerConfiguration = loggerConfiguration.WriteTo.Logger(lg =>
            lg.FilterSqlLog().Filter.ByIncludingOnly(Matching.WithProperty<bool>(LogContextStatic.SqlOutToFile, s => s))
                .WriteTo.Async(s => s.File(LogContextStatic.Combine(LogContextStatic.AopSql, @"AopSql.txt"), rollingInterval: RollingInterval.Day,
                    outputTemplate: LogContextStatic.FileMessageTemplate, retainedFileCountLimit: 31)));
        //输出普通日志
        loggerConfiguration = loggerConfiguration.WriteTo.Logger(lg =>
            lg.FilterRemoveSqlLog().WriteTo.Async(s => s.File(LogContextStatic.Combine(LogContextStatic.BasePathLogs, @"Log.txt"), rollingInterval: RollingInterval.Day,
                outputTemplate: LogContextStatic.FileMessageTemplate, retainedFileCountLimit: 31)));
        return loggerConfiguration;
    }

    public static LoggerConfiguration FilterSqlLog(this LoggerConfiguration lc)
    {
        lc = lc.Filter.ByIncludingOnly(Matching.WithProperty<string>(LogContextStatic.LogSource, s => LogContextStatic.AopSql.Equals(s)));
        return lc;
    }

    public static IEnumerable<LogEvent> FilterSqlLog(this IEnumerable<LogEvent> batch)
    {
        //只记录 Insert、Update、Delete语句
        return batch.Where(s => s.WithProperty<string>(LogContextStatic.LogSource, q => LogContextStatic.AopSql.Equals(q)))
            .Where(s => s.WithProperty<SugarActionType>(LogContextStatic.SugarActionType,
                q => !new[] { SugarActionType.UnKnown, SugarActionType.Query }.Contains(q)));
    }

    public static LoggerConfiguration FilterRemoveSqlLog(this LoggerConfiguration lc)
    {
        lc = lc.Filter.ByIncludingOnly(WithProperty<string>(LogContextStatic.LogSource, s => !LogContextStatic.AopSql.Equals(s)));
        return lc;
    }

    public static IEnumerable<LogEvent> FilterRemoveOtherLog(this IEnumerable<LogEvent> batch)
    {
        return batch.Where(s => WithProperty<string>(LogContextStatic.LogSource,
            q => !LogContextStatic.AopSql.Equals(q))(s));
    }

    public static Func<LogEvent, bool> WithProperty<T>(string propertyName, Func<T, bool> predicate)
    {
        //如果不包含属性 也认为是true
        return e =>
        {
            if (!e.Properties.TryGetValue(propertyName, out var propertyValue)) return true;

            return propertyValue is ScalarValue { Value: T value } && predicate(value);
        };
    }

    public static bool WithProperty<T>(this LogEvent e, string key, Func<T, bool> predicate)
    {
        if (!e.Properties.TryGetValue(key, out var propertyValue)) return false;

        return propertyValue is ScalarValue { Value: T value } && predicate(value);
    }
}