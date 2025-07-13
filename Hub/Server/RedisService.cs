using StackExchange.Redis;

namespace Hub.Server
{
    public class RedisService
    {
        private readonly IDatabase _redisDatabase;

        public RedisService(IConnectionMultiplexer redis)
        {
            _redisDatabase = redis.GetDatabase();
        }

        public async Task StoreTokenAsync(string connectionId, string token, TimeSpan expiry)
        {
            await _redisDatabase.StringSetAsync($"user:{connectionId}:token", token, expiry);
        }

        public async Task<string> GetTokenAsync(string connectionId)
        {
            return await _redisDatabase.StringGetAsync($"user:{connectionId}:token");
        }

        public async Task RemoveTokenAsync(string connectionId)
        {
            await _redisDatabase.KeyDeleteAsync($"user:{connectionId}:token");
        }

        public async Task<bool> IsTokenValidAsync(string connectionId, string token)
        {
            var storedToken = await GetTokenAsync(connectionId);
            return storedToken == token;
        }
    }

}
