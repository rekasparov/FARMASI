using FARMASI.Common.BaseEntities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FARMASI.Repository.Abstract
{
    public interface IBaseRepository<T, TKey>
        where T : IBaseEntity, new()
        where TKey : IEquatable<TKey>
    {
        IQueryable<T> Get(Expression<Func<T, bool>> predicate = null);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetByIdAsync(TKey id);
        Task<IList<T>> GetListByIdAsync(TKey id);
        Task<T> AddAsync(T entity);
        Task<bool> AddRangeAsync(string id, IEnumerable<T> entities);
        Task<T> UpdateAsync(TKey id, T entity);
        Task<T> DeleteAsync(T entity);
    }
}
