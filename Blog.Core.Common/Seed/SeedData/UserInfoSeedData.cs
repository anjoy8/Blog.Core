using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Core.Model.Models;
using SqlSugar;

namespace Blog.Core.Common.Seed.SeedData;

public class UserInfoSeedData : IEntitySeedData<SysUserInfo>
{
    public IEnumerable<SysUserInfo> InitSeedData()
    {
        return default;
    }

    public IEnumerable<SysUserInfo> SeedData()
    {
        return new[]
        {
            new SysUserInfo()
            {
                Id = 10001,
                LoginName = "zhangsan",
                LoginPWD = "E10ADC3949BA59ABBE56E057F20F883E",
                Name = "张三",
                TenantId = 1000001, //租户Id
            },
            new SysUserInfo()
            {
                Id = 10002,
                LoginName = "lisi",
                LoginPWD = "E10ADC3949BA59ABBE56E057F20F883E",
                Name = "李四",
                TenantId = 1000002, //租户Id
            },
        };
    }

    public Task CustomizeSeedData(ISqlSugarClient db)
    {
        return Task.CompletedTask;
    }
}