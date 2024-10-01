using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
namespace Shopping_API.Data;

public class RedisService
{
    private readonly ConnectionMultiplexer _redis;
    private readonly IDatabase _db;

    public RedisService(IConfiguration configuration)
    {
        _redis = ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis"));
        _db = _redis.GetDatabase();
    }

    public IDatabase GetDatabase()
    {
        return _db;
    }
}