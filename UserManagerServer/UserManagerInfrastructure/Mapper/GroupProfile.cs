using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using UserManagerCore.Dtos;
using UserManagerCore.Dtos.Group;
using UserManagerCore.Entities;

namespace UserManagerInfrastructure.Mapper
{
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            CreateMap<Group, GroupForDisplay>();
            CreateMap<GroupForCreate, Group>();
            CreateMap<GroupForUpdate, Group>().ReverseMap();
        }
    }
}
