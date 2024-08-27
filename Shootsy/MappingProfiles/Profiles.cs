using AutoMapper;
using Shootsy.Core.Interfaces;
using Shootsy.Database.Entities;
using Shootsy.Dtos;
using Shootsy.Models;

namespace Shootsy.MappingProfiles
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            CreateMap<CreateUserModel, UserDto>();
            CreateMap<UserDto, UserModelResponse>();
            CreateMap<IUser, UserDto>();
            CreateMap<UserDto, UserEntity>();
            CreateMap<CreateUserModel, UserDto>()
                .ForMember(dest => dest.Fullname, opt => opt.MapFrom(src => src.Firstname + ' ' + src.Lastname));
        }
    }
}
