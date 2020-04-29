using Blog.Core.Common;
using Blog.Core.Common.DB;
using Blog.Core.Common.LogHelper;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Core.Extensions
{
    /// <summary>
    /// SqlSugar 启动服务
    /// </summary>
    public static class SqlsugarSetup
    {
        public static void AddSqlsugarSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // 默认添加主数据库连接
            MainDb.CurrentDbConnId = Appsettings.app(new string[] { "MainDB" });

            // 把多个连接对象注入服务，这里必须采用Scope，因为有事务操作
            services.AddScoped<ISqlSugarClient>(o =>
            {
                // 连接字符串
                var listConfig = new List<ConnectionConfig>();
                // 从库
                var listConfig_Slave = new List<SlaveConnectionConfig>();
                BaseDBConfig.MutiConnectionString.Item2.ForEach(s =>
                {
                    listConfig_Slave.Add(new SlaveConnectionConfig()
                    {
                        HitRate = s.HitRate,
                        ConnectionString = s.Connection
                    });
                });

                BaseDBConfig.MutiConnectionString.Item1.ForEach(m =>
                {
                    listConfig.Add(new ConnectionConfig()
                    {
                        ConfigId = m.ConnId.ObjToString().ToLower(),
                        ConnectionString = m.Connection,
                        DbType = (DbType)m.DbType,
                        IsAutoCloseConnection = true,
                        IsShardSameThread = false,
                        AopEvents = new AopEvents
                        {
                            OnLogExecuting = (sql, p) =>
                            {
                                if (Appsettings.app(new string[] { "AppSettings", "SqlAOP", "Enabled" }).ObjToBool())
                                {
                                    Parallel.For(0, 1, e =>
                                    {
                                        MiniProfiler.Current.CustomTiming("SQL：", GetParas(p) + "【SQL语句】：" + sql);
                                        LogLock.OutSql2Log("SqlLog", new string[] { GetParas(p), "【SQL语句】：" + sql });

                                    });
                                }
                            }
                        },
                        MoreSettings = new ConnMoreSettings()
                        {
                            IsAutoRemoveDataCache = true
                        },
                        // 从库
                        SlaveConnectionConfigs = listConfig_Slave,
                        //InitKeyType = InitKeyType.SystemTable
                    }
                   );
                });
                return new SqlSugarClient(listConfig);
            });
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
}