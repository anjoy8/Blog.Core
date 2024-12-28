using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Core.Model.Models;
using Blog.Core.Model.Models.Tenant;
using SqlSugar;

namespace Blog.Core.Common.Seed.SeedData;

/// <summary>
/// 初始化 业务数据
/// </summary>
public class BusinessDataSeedData : IEntitySeedData<BusinessTable>
{
    public IEnumerable<BusinessTable> InitSeedData()
    {
        return new[]
        {
            new BusinessTable()
            {
                Id = 1,
                TenantId = 1000001,
                Name = "张三的数据01",
                Amount = 150,
                IsDeleted = true,
            },
            new BusinessTable()
            {
                Id = 2,
                TenantId = 1000001,
                Name = "张三的数据02",
                Amount = 200,
            },
            new BusinessTable()
            {
                Id = 3,
                TenantId = 1000001,
                Name = "张三的数据03",
                Amount = 250,
            },
            new BusinessTable()
            {
                Id = 4,
                TenantId = 1000002,
                Name = "李四的数据01",
                Amount = 300,
            },
            new BusinessTable()
            {
                Id = 5,
                TenantId = 1000002,
                Name = "李四的数据02",
                Amount = 500,
            },
            new BusinessTable()
            {
                Id = 6,
                TenantId = 0,
                Name = "公共数据01",
                Amount = 16600,
            },
            new BusinessTable()
            {
                Id = 7,
                TenantId = 0,
                Name = "公共数据02",
                Amount = 19800,
            },
        };
    }

    public IEnumerable<BusinessTable> SeedData()
    {
        return default;
    }

    public Task CustomizeSeedData(ISqlSugarClient db)
    {
        return Task.CompletedTask;
    }
}