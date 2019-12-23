using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserManagerCore.Dtos.Account;
using UserManagerCore.Entities;

namespace UserManagerCore.Services.Interfaces
{
    public interface IAccountService
    {
        Task<AccountInfo> CreateAccount(AccountForCreate model);

        Task UpdateAccount(AccountInfo model);

        Task DeleteAccount(AccountInfo model);
    }
}
