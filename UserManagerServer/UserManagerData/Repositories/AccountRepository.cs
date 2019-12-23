using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using UserManagerCore.Entities;
using UserManagerCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace UserManagerData.Repositories
{
    public class AccountRepository : BaseRepository<AccountInfo>, IAccountRepository
    {
        private readonly AppDbContext _context;

        public AccountRepository(AppDbContext context): base(context)
        {
            _context = context;
        }

        public async Task<AccountInfo> GetByIdAsync(int id, bool lazyLoading)
        {
            return (lazyLoading) switch
            {
                true => await _context.Set<AccountInfo>()
                    .Include(s => s.Group)
                    .SingleOrDefaultAsync(s => s.Id == id),

                false => await base.GetByIdAsync(id),
            };
        }

        public async Task<IList<AccountInfo>> GetWhereAsync(Expression<Func<AccountInfo, bool>> predicate, 
            bool lazyLoading)
        {
            return (lazyLoading) switch
            {
                true => await _context.Set<AccountInfo>()
                    .Include(s => s.Group)
                    .Where(predicate).ToListAsync(),

                false => await base.GetWhereAsync(predicate),
            };
        }
            
    }
}
