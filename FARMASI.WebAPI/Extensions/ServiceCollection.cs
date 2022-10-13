using FARMASI.Common.BaseSettings.MongoDB;
using FARMASI.Common.BaseSettings.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FARMASI.WebAPI.Extensions
{
    public static class ServiceCollection
    {
        public static IServiceCollection AddMongoDBSettings(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            return serviceCollection.Configure<MongoDBSettings>(config =>
            {
                config.ConnectionString = configuration.GetSection($"{typeof(MongoDBSettings).Name}:{nameof(MongoDBSettings.ConnectionString).ToLowerInvariant()}").Value;
                config.DatabaseName = configuration.GetSection($"{typeof(MongoDBSettings).Name}:{nameof(MongoDBSettings.DatabaseName)}".ToLowerInvariant()).Value;
            });
        }

        public static IServiceCollection AddRedisSettings(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            return serviceCollection.Configure<RedisSettings>(config =>
            {
                config.Host = configuration.GetSection($"{typeof(RedisSettings).Name}:{nameof(RedisSettings.Host).ToLowerInvariant()}").Value;
                config.Port = Convert.ToInt32(configuration.GetSection($"{typeof(RedisSettings).Name}:{nameof(RedisSettings.Port)}".ToLowerInvariant()).Value);
                config.Db = Convert.ToInt32(configuration.GetSection($"{typeof(RedisSettings).Name}:{nameof(RedisSettings.Db)}".ToLowerInvariant()).Value);
            });
        }
    }
}
