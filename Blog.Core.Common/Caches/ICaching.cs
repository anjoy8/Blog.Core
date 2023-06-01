using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace Blog.Core.Common.Caches;

/// <summary>
/// 缓存抽象接口,基于IDistributedCache封装
/// </summary>
public interface ICaching
{
	public IDistributedCache Cache { get; }
	void AddCacheKey(string cacheKey);
	Task AddCacheKeyAsync(string cacheKey);

	void DelByPattern(string key);
	Task DelByPatternAsync(string key);

	void DelCacheKey(string cacheKey);
	Task DelCacheKeyAsync(string cacheKey);

	bool Exists(string cacheKey);
	Task<bool> ExistsAsync(string cacheKey);

	List<string> GetAllCacheKeys();
	Task<List<string>> GetAllCacheKeysAsync();
	
	T Get<T>(string cacheKey);
	Task<T> GetAsync<T>(string cacheKey);

	string GetString(string cacheKey);
	Task<string> GetStringAsync(string cacheKey);

	void Remove(string key);
	Task RemoveAsync(string key);

	void RemoveAll();
	Task RemoveAllAsync();

	void Set<T>(string cacheKey, T value, TimeSpan? expire = null);
	Task SetAsync<T>(string cacheKey, T value);
	Task SetAsync<T>(string cacheKey, T value, TimeSpan expire);

	void SetPermanent<T>(string cacheKey, T value);
	Task SetPermanentAsync<T>(string cacheKey, T value);

	void SetString(string cacheKey, string value, TimeSpan? expire = null);
	Task SetStringAsync(string cacheKey, string value);
	Task SetStringAsync(string cacheKey, string value, TimeSpan expire);

	Task DelByParentKeyAsync(string key);
}