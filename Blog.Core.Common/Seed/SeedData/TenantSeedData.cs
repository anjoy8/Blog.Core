using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Core.Model.CustomEnums;
using Blog.Core.Model.Models;
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