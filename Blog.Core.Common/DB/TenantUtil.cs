using System;
using Blog.Core.Model.Models;
using SqlSugar;

namespace Blog.Core.Common.DB;

public static class TenantUtil
{
    public static ConnectionConfig GetConnectionConfig(this SysTenant tenant)
    {
        if (tenant.DbType is null)
        {
            throw new ArgumentException("Tenant DbType Must");
        }

        return new ConnectionConfig()
        {
            ConfigId = tenant.ConfigId,
            DbType = tenant.DbType.Value,
            ConnectionString = tenant.Connection,
            IsAutoCloseConnection = true,
            MoreSettings = new ConnMoreSettings()
            {
                IsAutoRemoveDataCache = true
            },
        };
    }
}