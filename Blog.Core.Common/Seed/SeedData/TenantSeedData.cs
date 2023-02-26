using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Blog.Core.Common.DB;
using Blog.Core.Model.Models;
using Blog.Core.Model.Tenants;
using SqlSugar;

namespace Blog.Core.Common.Seed.SeedData;

/// <summary>
/// 租户 种子数据
/// </summary>
public class TenantSeedData : IEntitySeedData<SysTenant>
{
    public IEnumerable<SysTenant> InitSeedData()
    {
        return new[]
        {
            new SysTenant()
            {
                Id = 1000001,
                ConfigId = "Tenant_1",
                Name = "张三",
                TenantType = TenantTypeEnum.Id
            },
            new SysTenant()
            {
                Id = 1000002,
                ConfigId = "Tenant_2",
                Name = "李四",
                TenantType = TenantTypeEnum.Id
            },
            new SysTenant()
            {
                Id = 1000003,
                ConfigId = "Tenant_3",
                Name = "王五",
                TenantType = TenantTypeEnum.Db,
                DbType = DbType.Sqlite,
                Connection = $"DataSource=" + Path.Combine(Environment.CurrentDirectory, "WangWu.db"),
            },
            new SysTenant()
            {
                Id = 1000004,
                ConfigId = "Tenant_4",
                Name = "赵六",
                TenantType = TenantTypeEnum.Db,
                DbType = DbType.Sqlite,
                Connection = $"DataSource=" + Path.Combine(Environment.CurrentDirectory, "ZhaoLiu.db"),
            },
            new SysTenant()
            {
                Id = 1000005,
                ConfigId = "Tenant_5",
                Name = "孙七",
                TenantType = TenantTypeEnum.Tables,
            },
        };
    }

    public IEnumerable<SysTenant> SeedData()
    {
        return default;
    }

    public Task CustomizeSeedData(ISqlSugarClient db)
    {
        return Task.CompletedTask;
    }
}