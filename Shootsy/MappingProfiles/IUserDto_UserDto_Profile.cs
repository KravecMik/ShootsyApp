using AutoMapper;
using Shootsy.Core.Interfaces;
using Shootsy.Dtos;

namespace Shootsy.MappingProfiles
{
    public class UserDto_IUser_Profile : Profile
    {
        public UserDto_IUser_Profile() =>
            CreateMap<UserDto, IUser>();
    }
}
