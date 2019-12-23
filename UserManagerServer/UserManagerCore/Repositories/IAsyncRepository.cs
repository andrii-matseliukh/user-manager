using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UserManagerCore.Entities;

namespace UserManagerCore.Repositories
{
    public interface IAsyncRepository<T> where T : Entity
    {
        Task<T> GetByIdAsync(int id);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<T> AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<IList<T>> GetAllAsync();
        Task<IList<T>> GetWhereAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
        Task SaveChangesAsync();
    }
}
