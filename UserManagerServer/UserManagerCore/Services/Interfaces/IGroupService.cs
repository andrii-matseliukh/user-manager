using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserManagerCore.Dtos.Group;
using UserManagerCore.Entities;

namespace UserManagerCore.Services.Interfaces
{
    public interface IGroupService
    {
        Task<Group> Create(GroupForCreate model);
        Task DeleteGroup(Group model);
        Task UpdateGroup(Group model);
    }
}
