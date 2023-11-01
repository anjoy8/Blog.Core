using System.IO;

namespace Blog.Core.Common.LogHelper;

public class LogContextStatic
{
    static LogContextStatic()
    {
        if (!Directory.Exists(BaseLogs))
        {
            Directory.CreateDirectory(BaseLogs);
        }
    }

    public static readonly string BaseLogs = "Logs";
    public static readonly string BasePathLogs = @"Logs";

    public static readonly string LogSource = "LogSource";
    public static readonly string AopSql = "AopSql";
    public static readonly string SqlOutToConsole = "OutToConsole";
    public static readonly string SqlOutToFile = "SqlOutToFile";
    public static readonly string OutToDb = "OutToDb";
    public static readonly string SugarActionType = "SugarActionType";

    public static readonly string FileMessageTemplate = "{NewLine}Date：{Timestamp:yyyy-MM-dd HH:mm:ss.fff}{NewLine}LogLevel：{Level}{NewLine}Message：{Message}{NewLine}{Exception}" + new string('-', 100);


    public static string Combine(string path1)
    {
        return Path.Combine(BaseLogs, path1);
    }

    public static string Combine(string path1, string path2)
    {
        return Path.Combine(BaseLogs, path1, path2);
    }

    public static string Combine(string path1, string path2, string path3)
    {
        return Path.Combine(BaseLogs, path1, path2, path3);
    }
}