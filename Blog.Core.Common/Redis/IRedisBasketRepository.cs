using System;
using System.Threading.Tasks;

namespace Blog.Core.Common
{
    /// <summary>
    /// Redis缓存接口
    /// </summary>
    public interface IRedisBasketRepository
    {

        //获取 Reids 缓存值
        Task<string> GetValue(string key);

        //获取值，并序列化
        Task<TEntity> Get<TEntity>(string key);

        //保存
        Task Set(string key, object value, TimeSpan cacheTime);

        //判断是否存在
        Task<bool> Exist(string key);

        //移除某一个缓存值
        Task Remove(string key);

        //全部清除
        Task Clear();
    }
}
