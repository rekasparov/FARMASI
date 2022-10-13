using FARMASI.Common.BaseEntities.Abstract;
using FARMASI.Common.BaseSettings.Redis;
using FARMASI.Repository.Abstract;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FARMASI.Repository.Concrete.Redis
{
    public class BaseRedisRepository<T> : IBaseRepository<T, string>
        where T : IBaseEntity, new()
    {
        protected readonly RedisClient redisClient;

        protected BaseRedisRepository(IOptions<RedisSettings> redisSettings)
        {
            redisClient = new RedisClient(new RedisEndpoint(redisSettings.Value.Host, redisSettings.Value.Port, db: redisSettings.Value.Db));
        }

        public async Task<T> AddAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddRangeAsync(string id, IEnumerable<T> entities)
        {
            byte[] buffer = Encoding.Default.GetBytes(id);
            string jsonValue = JsonConvert.SerializeObject(entities);
            byte[] setBuffer = Encoding.Default.GetBytes(jsonValue);
            redisClient.HSet(typeof(T).Name, buffer, setBuffer);
            return true;
        }

        public Task<T> DeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<T>> GetListByIdAsync(string id)
        {
            byte[] buffer = Encoding.Default.GetBytes(id);
            byte[] resultBuffer = redisClient.HGet(typeof(T).Name, buffer);
            if (resultBuffer == null)
                resultBuffer = Encoding.Default.GetBytes(JsonConvert.SerializeObject(new List<T>()));
            string jsonResult = Encoding.Default.GetString(resultBuffer);
            return JsonConvert.DeserializeObject<IList<T>>(jsonResult);
        }

        public Task<T> UpdateAsync(string id, T entity)
        {
            throw new NotImplementedException();
        }
    }
}
