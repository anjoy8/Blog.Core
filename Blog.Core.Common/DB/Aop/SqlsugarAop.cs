using Blog.Core.Common.LogHelper;
using Blog.Core.Model.Models.RootTkey;
using Blog.Core.Model.Tenants;
using SqlSugar;
using StackExchange.Profiling;
using System;
using Serilog;

namespace Blog.Core.Common.DB.Aop;

public static class SqlSugarAop
{
    public static void OnLogExecuting(ISqlSugarClient sqlSugarScopeProvider, string sql, SugarParameter[] p, ConnectionConfig config)
    {
        try
        {
            MiniProfiler.Current.CustomTiming($"ConnId:[{config.ConfigId}] SQL：", GetParas(p) + "【SQL语句】：" + sql);

            if (!AppSettings.app(new string[] { "AppSettings", "SqlAOP", "Enabled" }).ObjToBool()) return;

            if (AppSettings.app(new string[] { "AppSettings", "SqlAOP", "LogToConsole", "Enabled" }).ObjToBool() ||
                AppSettings.app(new string[] { "AppSettings", "SqlAOP", "LogToFile", "Enabled" }).ObjToBool() ||
                AppSettings.app(new string[] { "AppSettings", "SqlAOP", "LogToDB", "Enabled" }).ObjToBool())
            {
                using (LogContextExtension.Create.SqlAopPushProperty(sqlSugarScopeProvider))
                {
                    Log.Information("------------------ \r\n ConnId:[{ConnId}]【SQL语句】: \r\n {Sql}",
                        config.ConfigId, UtilMethods.GetSqlString(config.DbType, sql, p));
                }
            }
        }
        catch (Exception e)
        {
            Log.Error("Error occured OnLogExcuting:" + e);
        }
    }

    public static void DataExecuting(object oldValue, DataFilterModel entityInfo)
    {
        if (entityInfo.EntityValue is BaseEntity root)
        {
            if (root.Id == 0)
            {
                root.Id = SnowFlakeSingle.Instance.NextId();
            }
        }

        if (entityInfo.EntityValue is BaseEntity baseEntity)
        {
            // 新增操作
            if (entityInfo.OperationType == DataFilterType.InsertByObject)
            {
                if (baseEntity.CreateTime == DateTime.MinValue)
                {
                    baseEntity.CreateTime = DateTime.Now;
                }
            }

            if (entityInfo.OperationType == DataFilterType.UpdateByObject)
            {
                baseEntity.ModifyTime = DateTime.Now;
            }


            if (App.User?.ID > 0)
            {
                if (baseEntity is ITenantEntity tenant && App.User.TenantId > 0)
                {
                    if (tenant.TenantId == 0)
                    {
                        tenant.TenantId = App.User.TenantId;
                    }
                }

                switch (entityInfo.OperationType)
                {
                    case DataFilterType.UpdateByObject:
                        baseEntity.ModifyId = App.User.ID;
                        baseEntity.ModifyBy = App.User.Name;
                        break;
                    case DataFilterType.InsertByObject:
                        if (baseEntity.CreateBy.IsNullOrEmpty() || baseEntity.CreateId is null or <= 0)
                        {
                            baseEntity.CreateId = App.User.ID;
                            baseEntity.CreateBy = App.User.Name;
                        }

                        break;
                }
            }
        }
    }

    private static string GetWholeSql(SugarParameter[] paramArr, string sql)
    {
        foreach (var param in paramArr)
        {
            sql = sql.Replace(param.ParameterName, $@"'{param.Value.ObjToString()}'");
        }

        return sql;
    }

    private static string GetParas(SugarParameter[] pars)
    {
        string key = "【SQL参数】：";
        foreach (var param in pars)
        {
            key += $"{param.ParameterName}:{param.Value}\n";
        }

        return key;
    }
}