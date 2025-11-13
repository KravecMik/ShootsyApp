using AutoMapper;
using EnumsNET;
using Shootsy.Core.Interfaces;
using Shootsy.Database.Entities;
using Shootsy.Dtos;
using Shootsy.Enums;
using Shootsy.Models.User;
using Shootsy.Security;

namespace Shootsy.MappingProfiles
{
    public class UserProfiles : Profile
    {
        public UserProfiles()
        {
            CreateMap<SignUpRequestModel, UserDto>();
            CreateMap<UserDto, GetUserByIdResponse>()
                .ForMember(dest => dest.City, opt => opt.MapFrom(
                    src => ((CityEnum)src.City).AsString(EnumFormat.Description)))
                 .ForMember(dest => dest.ITProfession, opt => opt.MapFrom(
                    src => ((ITProfessionEnums)src.ITProfession).AsString(EnumFormat.Description)))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(
                    src => ((GenderEnums)src.Gender).AsString(EnumFormat.Description)));
            CreateMap<IUser, UserDto>();
            CreateMap<UserDto, UserEntity>();
            CreateMap<SignUpRequestModel, UserDto>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(
                    src => src.Password.EncryptString(src.Login)));
        }
    }
}