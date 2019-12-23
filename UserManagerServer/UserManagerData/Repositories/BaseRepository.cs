using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UserManagerCore.Entities;
using UserManagerCore.Repositories;

namespace UserManagerData.Repositories
{
    public class BaseRepository<T> : IAsyncRepository<T>
        where T : Entity
    {
        private readonly AppDbContext _context;

        public BaseRepository(AppDbContext context)
        { 
            this._context = context;
        }

        public async Task<T> AddAsync(T entity)
        {
            var model = await _context.Set<T>().AddAsync(entity);
            return model.Entity;
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        { 
            return await _context.Set<T>().CountAsync(predicate);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        { 
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public async Task<IList<T>> GetAllAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IList<T>> GetWhereAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
