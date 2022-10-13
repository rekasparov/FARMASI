using FARMASI.Common.BaseSettings.Redis;
using FARMASI.DataAccessLayer.Abstract;
using FARMASI.Entity;
using FARMASI.Repository.Concrete.Redis;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FARMASI.DataAccessLayer.Concrete
{
    public class ShoppingCartDAL : BaseRedisRepository<ShoppingCart>, IShoppingCartDAL
    {
        public ShoppingCartDAL(IOptions<RedisSettings> redisSettings)
            : base(redisSettings)
        {
        }
    }
}
