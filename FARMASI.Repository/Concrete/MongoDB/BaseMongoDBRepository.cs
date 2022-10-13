using FARMASI.Common.BaseEntities.Abstract;
using FARMASI.Common.BaseSettings.MongoDB;
using FARMASI.Repository.Abstract;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FARMASI.Repository.Concrete.MongoDB
{
    public class BaseMongoDBRepository<T> : IBaseRepository<T, string>
        where T : IBaseEntity, new()
    {
        protected readonly IMongoCollection<T> mongoCollection;

        protected BaseMongoDBRepository(IOptions<MongoDBSettings> mongoDBSettings)
        {
            IMongoClient mongoClient = new MongoClient(mongoDBSettings.Value.ConnectionString);

            IMongoDatabase mongoDatabase = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);

            mongoCollection = mongoDatabase.GetCollection<T>(typeof(T).Name);
        }

        public virtual IQueryable<T> Get(Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null
                ? mongoCollection.AsQueryable()
                : mongoCollection.AsQueryable().Where(predicate);
        }

        public virtual Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return mongoCollection.Find(predicate).FirstOrDefaultAsync();
        }

        public virtual Task<T> GetByIdAsync(string id)
        {
            return mongoCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            var options = new InsertOneOptions { BypassDocumentValidation = false };
            await mongoCollection.InsertOneAsync(entity, options);
            return entity;
        }

        public virtual async Task<bool> AddRangeAsync(string id, IEnumerable<T> entities)
        {
            var options = new BulkWriteOptions { IsOrdered = false, BypassDocumentValidation = false };
            return (await mongoCollection.BulkWriteAsync((IEnumerable<WriteModel<T>>)entities, options)).IsAcknowledged;
        }

        public virtual async Task<T> UpdateAsync(string id, T entity)
        {
            return await mongoCollection.FindOneAndReplaceAsync(x => x.Id == id, entity);
        }

        public virtual async Task<T> DeleteAsync(T entity)
        {
            return await mongoCollection.FindOneAndDeleteAsync(x => x.Id == entity.Id);
        }

        public Task<IList<T>> GetListByIdAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
