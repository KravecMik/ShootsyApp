using AutoMapper;
using Shootsy.Database.Entities;
using Shootsy.Dtos;

namespace Shootsy.MappingProfiles
{
    public class UserEntity_UserDto_Profile : Profile
    {
        public UserEntity_UserDto_Profile() =>
            CreateMap<UserEntity, UserDto>();

    }
}
