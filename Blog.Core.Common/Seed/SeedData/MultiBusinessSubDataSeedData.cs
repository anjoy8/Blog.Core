using Blog.Core.Model.Models;
using SqlSugar;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Core.Model.Models.Tenant;

namespace Blog.Core.Common.Seed.SeedData;

public class MultiBusinessSubDataSeedData : IEntitySeedData<MultiBusinessSubTable>
{
    public IEnumerable<MultiBusinessSubTable> InitSeedData()
    {
        return new List<MultiBusinessSubTable>()
        {
            new()
            {
                Id = 100,
                MainId = 1001,
                Memo = "子数据",
            },
            new()
            {
                Id = 1001,
                MainId = 1001,
                Memo = "子数据2",
            },
        };
    }

    public IEnumerable<MultiBusinessSubTable> SeedData()
    {
        return default;
    }

    public Task CustomizeSeedData(ISqlSugarClient db)
    {
        return Task.CompletedTask;
    }
}