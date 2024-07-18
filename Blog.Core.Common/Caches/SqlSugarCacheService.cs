using System;
using System.Collections.Generic;
using Blog.Core.Common.Caches.Interface;
using SqlSugar;

namespace Blog.Core.Common.Caches;

/// <summary>
/// 实现SqlSugar的ICacheService接口
/// </summary>
public class SqlSugarCacheService : ICacheService
{
	private readonly Lazy<ICaching> _caching = new(() => App.GetService<ICaching>(false));
	private ICaching Caching => _caching.Value;

	public void Add<V>(string key, V value)
	{
		Caching.Set(key, value);
	}

	public void Add<V>(string key, V value, int cacheDurationInSeconds)
	{
		Caching.Set(key, value, TimeSpan.FromSeconds(cacheDurationInSeconds));
	}

	public bool ContainsKey<V>(string key)
	{
		return Caching.Exists(key);
	}

	public V Get<V>(string key)
	{
		return Caching.Get<V>(key);
	}

	public IEnumerable<string> GetAllKey<V>()
	{
		return Caching.GetAllCacheKeys();
	}

	public V GetOrCreate<V>(string cacheKey, Func<V> create, int cacheDurationInSeconds = int.MaxValue)
	{
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
		Caching.Remove(key);
	}

	public bool RemoveAll()
	{
		Caching.RemoveAll();
		return true;
	}
}