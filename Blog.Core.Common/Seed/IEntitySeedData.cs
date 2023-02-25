using SqlSugar;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Core.Common.Seed;

/// <summary>
/// 种子数据 接口
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IEntitySeedData<out T>
    where T : class, new()
{
    /// <summary>
    /// 初始化种子数据 <br/>
    /// 只要表不存在数据,程序启动就会自动初始化
    /// </summary>
    /// <returns></returns>
    IEnumerable<T> InitSeedData();

    /// <summary>
    /// 种子数据 <br/>
    /// 存在不操作、不存在Insert <br/>
    /// 适合系统内置数据,项目开发后续增加内置数据
    /// </summary>
    /// <returns></returns>
    IEnumerable<T> SeedData();

    /// <summary>
    /// 自定义操作 <br/>
    /// 以上满不足了,可以自己编写
    /// </summary>
    /// <param name="db"></param>
    /// <returns></returns>
    Task CustomizeSeedData(ISqlSugarClient db);
}