using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using UserManagerCore.Dtos;
using UserManagerCore.Dtos.Account;
using UserManagerCore.Entities;

namespace UserManagerInfrastructure.Mapper
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<AccountForUpdate, AccountInfo>().ReverseMap();
            CreateMap<AccountForCreate, AccountInfo>();
            CreateMap<AccountInfo, AccountForDisplay>();
            
        }
    }

    //CreateMap<CatalogItem, ProductInfoDto>()
    //    .ForMember(s => s.ProductName, o => o.MapFrom(o => o.Name))
    //    .ForMember(s => s.ImagesSource, o => o.MapFrom(o => o.ImageAssets.Select(s => s.ImagePath)));

}
