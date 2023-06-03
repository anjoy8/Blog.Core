using Serilog.Context;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace Blog.Core.Common.LogHelper;

public class LogContextExtension : IDisposable
{
    private readonly Stack<IDisposable> _disposableStack = new Stack<IDisposable>();

    public static LogContextExtension Create => new();

    public void AddStock(IDisposable disposable)
    {
        _disposableStack.Push(disposable);
    }

    public IDisposable SqlAopPushProperty(ISqlSugarClient db)
    {
        AddStock(LogContext.PushProperty(LogContextStatic.LogSource, LogContextStatic.AopSql));
        AddStock(LogContext.PushProperty(LogContextStatic.SqlOutToConsole,
            AppSettings.app(new string[] { "AppSettings", "SqlAOP", "LogToConsole", "Enabled" }).ObjToBool()));
        AddStock(LogContext.PushProperty(LogContextStatic.SqlOutToFile,
            AppSettings.app(new string[] { "AppSettings", "SqlAOP", "LogToFile", "Enabled" }).ObjToBool()));
        AddStock(LogContext.PushProperty(LogContextStatic.OutToDb,
            AppSettings.app(new string[] { "AppSettings", "SqlAOP", "LogToDB", "Enabled" }).ObjToBool()));

        AddStock(LogContext.PushProperty(LogContextStatic.SugarActionType, db.SugarActionType));

        return this;
    }


    public void Dispose()
    {
        while (_disposableStack.Count > 0)
        {
            _disposableStack.Pop().Dispose();
        }
    }
}