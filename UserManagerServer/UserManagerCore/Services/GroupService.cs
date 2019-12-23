using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserManagerCore.Dtos.Group;
using UserManagerCore.Entities;
using UserManagerCore.Repositories;
using UserManagerCore.Services.Interfaces;

namespace UserManagerCore.Services
{
    public class GroupService : IGroupService
    {
        private readonly IMapper _mapper;
        private readonly IAsyncRepository<Group> _groupRepository;
        private readonly IAccountRepository _accountRepository;

        public GroupService(IMapper mapper,
            IAsyncRepository<Group> groupRepository,
            IAccountRepository accountRepository)
        {
            _mapper = mapper;
            _groupRepository = groupRepository;
            _accountRepository = accountRepository;
        }

        public async Task<Group> Create(GroupForCreate model)
        {
            var groupEntity = _mapper.Map<Group>(model);

            await _groupRepository.AddAsync(groupEntity);

            await _groupRepository.SaveChangesAsync();

            return groupEntity;
        }

        public async Task DeleteGroup(Group model)
        {
            _groupRepository.Delete(model);
            await _groupRepository.SaveChangesAsync();

        }

        public async Task UpdateGroup(Group model)
        {
            _groupRepository.Update(model);
            await _accountRepository.SaveChangesAsync();
        }
    }
}
