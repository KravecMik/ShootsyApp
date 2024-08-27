using AutoMapper;
using Shootsy.Dtos;
using Shootsy.Models;

namespace Shootsy.MappingProfiles
{
    public class CreateUserModel_UserDto_Profle : Profile
    {
        public CreateUserModel_UserDto_Profle() =>
            CreateMap<CreateUserModel, UserDto>()
            .ReverseMap();

    }
}
