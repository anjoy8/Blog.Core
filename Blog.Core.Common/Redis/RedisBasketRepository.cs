using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Core.Common
{
    public class RedisBasketRepository : IRedisBasketRepository
    {
        private readonly ILogger<RedisBasketRepository> _logger;
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;

        public RedisBasketRepository(ILogger<RedisBasketRepository> logger, ConnectionMultiplexer redis)
        {
            _logger = logger;
            _redis = redis;
            _database = redis.GetDatabase();
        }

        private IServer GetServer()
        {
            var endpoint = _redis.GetEndPoints();
            return _redis.GetServer(endpoint.First());
        }

        public async Task Clear()
        {
            foreach (var endPoint in _redis.GetEndPoints())
            {
                var server = GetServer();
                foreach (var key in server.Keys())
                {
                    await _database.KeyDeleteAsync(key);
                }
            }
        }

        public async Task<bool> Exist(string key)
        {
            return await _database.KeyExistsAsync(key);
        }

        public async Task<string> GetValue(string key)
        {
            return await _database.StringGetAsync(key);
        }

        public async Task Remove(string key)
        {
            await _database.KeyDeleteAsync(key);
        }

        public async Task Set(string key, object value, TimeSpan cacheTime)
        {
            if (value != null)
            {
                //序列化，将object值生成RedisValue
               await _database.StringSetAsync(key, SerializeHelper.Serialize(value), cacheTime);
            }
        }

        public async Task<TEntity> Get<TEntity>(string key)
        {
            var value = await _database.StringGetAsync(key);
            if (value.HasValue)
            {
                //需要用的反序列化，将Redis存储的Byte[]，进行反序列化
                return SerializeHelper.Deserialize<TEntity>(value);
            }
            else
            {
                return default(TEntity);
            }
        }
    }
}
