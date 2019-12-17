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

            // 把多个连接对象注入服务，这里可以采用Transient瞬态，也可以Scope
            services.AddScoped(o =>
            {
                List<ISqlSugarClient> sqlSugarClients = new List<ISqlSugarClient>();

                var beforeDbConnect = BaseDBConfig.MutiConnectionString;
                var index = beforeDbConnect.FindIndex(x => x.ConnId == MainDb.CurrentDbConnId);
                if (index > 0)
                {
                    // 做一个交换，切换主db连接放到第一位
                    var firstDb = beforeDbConnect[0];
                    var changeDb = beforeDbConnect[index];
                    beforeDbConnect[index] = firstDb;
                    beforeDbConnect[0] = changeDb;
                }

                beforeDbConnect.ForEach(m =>
                {
                    sqlSugarClients.Add(new SqlSugarClient(new ConnectionConfig()
                    {
                        ConfigId = m.ConnId,
                        ConnectionString = m.Conn,
                        DbType = (DbType)m.DbType,
                        IsAutoCloseConnection = true,
                        //InitKeyType = InitKeyType.SystemTable
                    })
                   );
                });
                return sqlSugarClients;
            });
        }
    }
}
