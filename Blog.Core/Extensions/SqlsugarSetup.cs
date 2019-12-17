using Blog.Core.Common;
using Blog.Core.Common.DB;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System;
using System.Collections.Generic;

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

            services.AddSingleton(new MainDb(Appsettings.app(new string[] { "MainDB" }).ObjToInt()));

            services.AddScoped<ISqlSugarClient>(o =>
            {
                List<ConnectionConfig> connectionConfigs = new List<ConnectionConfig>();
                BaseDBConfig.MutiConnectionString.ForEach(m =>
                {
                    connectionConfigs.Add(new ConnectionConfig()
                    {
                        ConfigId = m.ConnId,
                        ConnectionString = m.Conn,
                        DbType = (DbType)m.DbType,
                        IsAutoCloseConnection = true,
                        //InitKeyType = InitKeyType.SystemTable
                    });
                });
                return new SqlSugarClient(connectionConfigs);
            });

        }
    }
}
