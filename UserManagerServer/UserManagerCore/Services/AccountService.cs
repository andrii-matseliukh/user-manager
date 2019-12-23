using System;
using System.Threading.Tasks;

using AutoMapper;

using UserManagerCore.Dtos.Account;
using UserManagerCore.Entities;
using UserManagerCore.Repositories;
using UserManagerCore.Services.Interfaces;

namespace UserManagerCore.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public AccountService(IMapper mapper,
            IAccountRepository accountRepository)
        {
            _mapper = mapper;
            _accountRepository = accountRepository;
        }
        public async Task<AccountInfo> CreateAccount(AccountForCreate model)
        {
            var accountEntity = _mapper.Map<AccountInfo>(model);

            await _accountRepository.AddAsync(accountEntity);

            await _accountRepository.SaveChangesAsync();

            return accountEntity;
        }

        public async Task UpdateAccount(AccountInfo model)
        {
            _accountRepository.Update(model);

            await _accountRepository.SaveChangesAsync();
        }

        public async Task DeleteAccount(AccountInfo model)
        {
            _accountRepository.Delete(model);

            await _accountRepository.SaveChangesAsync();
        }
    }
}
