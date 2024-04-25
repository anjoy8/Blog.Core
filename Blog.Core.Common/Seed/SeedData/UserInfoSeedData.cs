using System.Collections.Generic;
using System.Linq;
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
        return default;
    }

    public async Task CustomizeSeedData(ISqlSugarClient db)
    {
        var data = new List<SysUserInfo>()
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
            new SysUserInfo()
            {
                Id = 10003,
                LoginName = "wangwu",
                LoginPWD = "E10ADC3949BA59ABBE56E057F20F883E",
                Name = "王五",
                TenantId = 1000003, //租户Id
            },
            new SysUserInfo()
            {
                Id = 10004,
                LoginName = "zhaoliu",
                LoginPWD = "E10ADC3949BA59ABBE56E057F20F883E",
                Name = "赵六",
                TenantId = 1000004, //租户Id
            },
            new SysUserInfo()
            {
                Id = 10005,
                LoginName = "sunqi",
                LoginPWD = "E10ADC3949BA59ABBE56E057F20F883E",
                Name = "孙七",
                TenantId = 1000005, //租户Id
            },
        };

        var names = data.Select(s => s.LoginName).ToList();
        names = await db.Queryable<SysUserInfo>()
            .Where(s => names.Contains(s.LoginName))
            .Select(s => s.LoginName).ToListAsync();

        var sysUserInfos = data.Where(s => !names.Contains(s.LoginName)).ToList();
        if (sysUserInfos.Any())
        {
            //await db.Insertable<SysUserInfo>(sysUserInfos).ExecuteReturnIdentityAsync();//postgresql这句会报错
            await db.Insertable<SysUserInfo>(sysUserInfos).ExecuteCommandAsync();
        }

        await Task.CompletedTask;
    }
}
