using AutoMapper;
using Shootsy.Database.Entities;
using Shootsy.Dtos;

namespace Shootsy.MappingProfiles
{
    public class UserEntityArray_UserDtoList_Profile : Profile
    {
        public UserEntityArray_UserDtoList_Profile() =>
            CreateMap<UserEntity[], List<UserDto>>();
    }
}
