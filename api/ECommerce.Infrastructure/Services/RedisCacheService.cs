using System;
using System.Text.Json;
using System.Threading.Tasks;
using ECommerce.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace ECommerce.Infrastructure.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _db;

        public RedisCacheService(IConfiguration configuration)
        {
            var redisConnection = configuration.GetConnectionString("Redis");
            if (string.IsNullOrEmpty(redisConnection))
                throw new ArgumentNullException(nameof(redisConnection), "Redis connection string is null");

            var redis = ConnectionMultiplexer.Connect(redisConnection);
            _db = redis.GetDatabase();
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _db.StringGetAsync(key);
            return value.IsNullOrEmpty ? default : JsonSerializer.Deserialize<T>(value);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var json = JsonSerializer.Serialize(value);
            await _db.StringSetAsync(key, json, expiration);
        }

        public async Task RemoveAsync(string key)
        {
            await _db.KeyDeleteAsync(key);
        }

        public async Task RemoveByPatternAsync(string pattern)
        {
            var server = _db.Multiplexer.GetServer(_db.Multiplexer.GetEndPoints()[0]);
            foreach (var key in server.Keys(pattern: pattern))
            {
                await _db.KeyDeleteAsync(key);
            }
        }
    }
}
