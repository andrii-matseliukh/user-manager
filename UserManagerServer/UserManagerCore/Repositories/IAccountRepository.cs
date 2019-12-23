using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UserManagerCore.Entities;

namespace UserManagerCore.Repositories
{
    public interface IAccountRepository: IAsyncRepository<AccountInfo>
    {
        Task<IList<AccountInfo>> GetWhereAsync(Expression<Func<AccountInfo, bool>> predicate, bool lazyLoading);
        Task<AccountInfo> GetByIdAsync(int id, bool lazyLoading);
    }
}
