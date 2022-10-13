using FARMASI.Common.BaseSettings.Redis;
using FARMASI.DataAccessLayer.Abstract;
using FARMASI.DataAccessLayer.Concrete;
using FARMASI.UnitOfWork.Abstract.Redis;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FARMASI.UnitOfWork.Concrete.Redis
{
    public class BaseRedisUnitOfWork : IBaseRedisUnitOfWork
    {
        private readonly IOptions<RedisSettings> redisSettings;

        public BaseRedisUnitOfWork(IOptions<RedisSettings> redisSettings)
        {
            this.redisSettings = redisSettings;
        }

        public IShoppingCartDAL ShoppingCart => new ShoppingCartDAL(redisSettings);
    }
}
