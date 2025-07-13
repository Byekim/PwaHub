using StackExchange.Redis;
using System.Threading.Tasks;

namespace Hub.Server.Common
{
    public class RedisClient
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisClient(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task<string> GetAsync(string key)
        {
            var db = _redis.GetDatabase();
            return await db.StringGetAsync(key);
        }

        public async Task SetAsync(string key, string value)
        {
            var db = _redis.GetDatabase();
            await db.StringSetAsync(key, value);
        }
    }

}
