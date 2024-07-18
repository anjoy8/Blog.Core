using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Blog.Core.Common.Caches.Distributed;

/// <summary>
/// A common memory distributed cache.<br/>
/// 因为微软的MemoryDistributedCache内部自己实例化MemoryCache,而不是使用IMemoryCache接口
/// </summary>
public class CommonMemoryDistributedCache : IDistributedCache
{
    private readonly IMemoryCache _memCache;

    public CommonMemoryDistributedCache(IOptions<MemoryDistributedCacheOptions> optionsAccessor, IMemoryCache memoryCache)
        : this(optionsAccessor, NullLoggerFactory.Instance, memoryCache)
    {
    }

    public CommonMemoryDistributedCache(IOptions<MemoryDistributedCacheOptions> optionsAccessor, ILoggerFactory loggerFactory,
        IMemoryCache memoryCache)
    {
        ArgumentNullException.ThrowIfNull(optionsAccessor);
        ArgumentNullException.ThrowIfNull(loggerFactory);

        _memCache = memoryCache ?? new MemoryCache(optionsAccessor.Value, loggerFactory);
    }

    public byte[] Get(string key)
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        return (byte[])_memCache.Get(key);
    }

    public Task<byte[]> GetAsync(string key, CancellationToken token = default(CancellationToken))
    {
        ArgumentNullException.ThrowIfNull(key);
        return Task.FromResult(Get(key));
    }

    public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(options);

        var memoryCacheEntryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpiration = options.AbsoluteExpiration,
            AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow,
            SlidingExpiration = options.SlidingExpiration,
            Size = value.Length
        };

        _memCache.Set(key, value, memoryCacheEntryOptions);
    }

    public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken))
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(options);

        Set(key, value, options);
        return Task.CompletedTask;
    }

    public void Refresh(string key)
    {
        ArgumentNullException.ThrowIfNull(key);

        _memCache.TryGetValue(key, out object value);
    }

    public Task RefreshAsync(string key, CancellationToken token = default(CancellationToken))
    {
        ArgumentNullException.ThrowIfNull(key);

        Refresh(key);
        return Task.CompletedTask;
    }

    public void Remove(string key)
    {
        ArgumentNullException.ThrowIfNull(key);

        _memCache.Remove(key);
    }

    public Task RemoveAsync(string key, CancellationToken token = default(CancellationToken))
    {
        ArgumentNullException.ThrowIfNull(key);

        Remove(key);
        return Task.CompletedTask;
    }
}