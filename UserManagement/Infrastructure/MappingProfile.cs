using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Service.DTOModels;
using UserManagement.Service.Models;
using UserManagement.ViewModels;

namespace UserManagement.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BasicUserDTO, BasicUserViewModel>();
            CreateMap<BasicUserViewModel, BasicUserDTO>();

            CreateMap<FullUserDTO, FullUserViewModel>();
            CreateMap<FullUserViewModel, FullUserDTO>();

            CreateMap<PhoneDTO, PhoneViewModel>();
            CreateMap<PhoneViewModel, PhoneDTO>();

            CreateMap<RelatedPersonReportDTO, RelatedPersonReportViewModel>();
            //CreateMap<RelatedPersonReportViewModel, RelatedPersonReportDTO>();

            CreateMap<RelatedUserDto, RelatedUserViewModel>();
            CreateMap<RelatedUserViewModel, RelatedUserDto>();
        }
    }
}
