using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using RabbitMQOrderSystem.Application.Interfaces;
using System.Text.Json;

namespace RabbitMQOrderSystem.Infrastructure.Messaging.Cache
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<RedisCacheService> _logger;

        public RedisCacheService(IDistributedCache cache, ILogger<RedisCacheService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _cache.GetStringAsync(key);
            if (value == null) return default;

            _logger.LogInformation("Cache Hit for key: {Key}", key);
            return JsonSerializer.Deserialize<T>(value);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromMinutes(30)
            };

            var json = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, json, options);

            _logger.LogInformation("Cache Set for key: {Key}", key);
        }

        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
            _logger.LogInformation("Cache Removed for key: {Key}", key);
        }

        public async Task<bool> ExistsAsync(string key)
        {
            return await _cache.GetStringAsync(key) != null;
        }
    }
}
