using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Model.Data.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IQueryable<T>> GetByConditionAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task AddRangeAsync(List<T> entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        int Count(Expression<Func<T, bool>> predicate = null);
        Task SaveChangesAsync();
    }
}
