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
            CreateMap<SignUpRequest, UserDto>();
            CreateMap<UserDto, GetUserByIdResponse>()
                .ForMember(dest => dest.City, opt => opt.MapFrom(
                    src => ((CityEnum)src.City).AsString(EnumFormat.Description)))
                 .ForMember(dest => dest.Type, opt => opt.MapFrom(
                    src => ((UserTypeEnums)src.Type).AsString(EnumFormat.Description)))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(
                    src => ((GenderEnums)src.Gender).AsString(EnumFormat.Description)));
            CreateMap<IUser, UserDto>();
            CreateMap<UserDto, UserEntity>();
            CreateMap<SignUpRequest, UserDto>()
                .ForMember(dest => dest.Fullname, opt => opt.MapFrom(
                    src => src.Patronymic != null ?
                        src.Firstname + ' ' + src.Lastname + ' ' + src.Patronymic :
                            src.Firstname + ' ' + src.Lastname))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(
                    src => src.Password.EncryptString(src.Login)));
        }
    }
}