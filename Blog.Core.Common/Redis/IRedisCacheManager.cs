using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Common
{
    /// <summary>
    /// Redis缓存接口
    /// </summary>
    public interface IRedisCacheManager
    {

        //获取 Reids 缓存值
        string GetValue(string key);

        //获取值，并序列化
        TEntity Get<TEntity>(string key);

        //保存
        void Set(string key, object value, TimeSpan cacheTime);

        //判断是否存在
        bool Get(string key);

        //移除某一个缓存值
        void Remove(string key);

        //全部清除
        void Clear();
    }
}
