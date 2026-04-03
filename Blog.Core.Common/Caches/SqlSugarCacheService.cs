using System;
using System.Collections.Generic;
using Blog.Core.Common.Caches.Interface;
using Blog.Core.Common.Option;
using SqlSugar;

namespace Blog.Core.Common.Caches;

/// <summary>
/// 实现SqlSugar的ICacheService接口<br/>
/// <br/>
/// 建议自己实现业务缓存,注入ICaching直接用即可<br/>
/// <br/>
/// 不建议使用SqlSugar缓存,性能有很大问题,会导致redis堆积<br/>
/// 核心问题在于SqlSugar，每次query（注意：不管你有没有启用,所有表的查询）都会查缓存, insert\update\delete,又会频繁GetAllKey，导致性能特别低<br/>
/// </summary>
public class SqlSugarCacheService : ICacheService
{
    private readonly Lazy<ICaching> _caching = new(() => App.GetService<ICaching>(false));
    private ICaching Caching => _caching.Value;
    private readonly AppSettingsOption _options = App.GetOptions<AppSettingsOption>();

    public void Add<V>(string key, V value)
    {
        if (!_options.CacheDbEnabled)
        {
            return;
        }

        Caching.Set(key, value);
    }

    public void Add<V>(string key, V value, int cacheDurationInSeconds)
    {
        if (!_options.CacheDbEnabled)
        {
            return;
        }

        Caching.Set(key, value, TimeSpan.FromSeconds(cacheDurationInSeconds));
    }

    public bool ContainsKey<V>(string key)
    {
        if (!_options.CacheDbEnabled)
        {
            return default;
        }

        return Caching.Exists(key);
    }

    public V Get<V>(string key)
    {
        if (!_options.CacheDbEnabled)
        {
            return default;
        }

        return Caching.Get<V>(key);
    }

    public IEnumerable<string> GetAllKey<V>()
    {
        if (!_options.CacheDbEnabled)
        {
            return default;
        }

        return Caching.GetAllCacheKeys();
    }

    public V GetOrCreate<V>(string cacheKey, Func<V> create, int cacheDurationInSeconds = int.MaxValue)
    {
        if (!_options.CacheDbEnabled)
        {
            return create();
        }

        if (!ContainsKey<V>(cacheKey))
        {
            var value = create();
            Caching.Set(cacheKey, value, TimeSpan.FromSeconds(cacheDurationInSeconds));
            return value;
        }

        return Caching.Get<V>(cacheKey);
    }

    public void Remove<V>(string key)
    {
        if (!_options.CacheDbEnabled)
        {
            return;
        }

        Caching.Remove(key);
    }

    public bool RemoveAll()
    {
        if (!_options.CacheDbEnabled)
        {
            return true;
        }

        Caching.RemoveAll();
        return true;
    }
}