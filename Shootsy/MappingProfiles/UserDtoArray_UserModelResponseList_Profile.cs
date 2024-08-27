using AutoMapper;
using Shootsy.Database.Entities;
using Shootsy.Models;

namespace Shootsy.MappingProfiles
{
    public class UserDtoArray_UserModelResponseList_Profile : Profile
    {
        public UserDtoArray_UserModelResponseList_Profile() =>
            CreateMap<UserEntity[], List<UserModelResponse>>();
    }
}
