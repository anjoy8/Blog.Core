using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Blog.Core.Model.Base.Tenants;
using Blog.Core.Model.Models;
using SqlSugar;

namespace Blog.Core.Common.DB;

public static class TenantUtil
{
    public static SysTenant DefaultTenantConfig(this SysTenant tenant)
    {
        tenant.DbType ??= DbType.Sqlite;

        //如果没有配置连接
        if (tenant.Connection.IsNullOrEmpty())
        {
            //此处默认配置 Sqlite 地址
            //实际业务中 也会有运维、系统管理员等来维护
            switch (tenant.DbType.Value)
            {
                case DbType.Sqlite:
                    tenant.Connection = $"DataSource={Path.Combine(Environment.CurrentDirectory, tenant.ConfigId)}.db";
                    break;
            }
        }

        return tenant;
    }

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
                IsAutoRemoveDataCache = true,
                SqlServerCodeFirstNvarchar = true,
            },
        };
    }

    public static List<Type> GetTenantEntityTypes(TenantTypeEnum? tenantType = null)
    {
        return RepositorySetting.Entitys
            .Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass)
            .Where(s => IsTenantEntity(s, tenantType))
            .ToList();
    }

    public static bool IsTenantEntity(this Type u, TenantTypeEnum? tenantType = null)
    {
        var mta = u.GetCustomAttribute<MultiTenantAttribute>();
        if (mta is null)
        {
            return false;
        }

        if (tenantType != null)
        {
            if (mta.TenantType != tenantType)
            {
                return false;
            }
        }

        return true;
    }

    public static string GetTenantTableName(this Type type, ISqlSugarClient db, string id)
    {
        var entityInfo = db.EntityMaintenance.GetEntityInfo(type);
        return $@"{entityInfo.DbTableName}_{id}";
    }

    public static string GetTenantTableName(this Type type, ISqlSugarClient db, SysTenant tenant)
    {
        return GetTenantTableName(type, db, tenant.Id.ToString());
    }

    public static void SetTenantTable(this ISqlSugarClient db, string id)
    {
        var types = GetTenantEntityTypes(TenantTypeEnum.Tables);

        foreach (var type in types)
        {
            db.MappingTables.Add(type.Name, type.GetTenantTableName(db, id));
        }
    }
}