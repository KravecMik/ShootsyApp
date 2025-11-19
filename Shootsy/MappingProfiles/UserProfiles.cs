using AutoMapper;
using Shootsy.Database.Entities;
using Shootsy.Models.Dtos;
using Shootsy.Models.User;

namespace Shootsy.MappingProfiles
{
    public class UserProfiles : Profile
    {
        public UserProfiles()
        {
            CreateMap<UserEntity, UserDto>()
                 .ForMember(dest => dest.City,
                       opt => opt.MapFrom(src => src.CityEntity.CityName))
                 .ForMember(dest => dest.Profession,
                       opt => opt.MapFrom(src => src.ProfessionEntity.Name))
                 .ForMember(dest => dest.Category,
                       opt => opt.MapFrom(src => src.ProfessionEntity.Category))
                 .ForMember(dest => dest.Gender,
                       opt => opt.MapFrom(src => src.GenderEntity.GenderName));

            CreateMap<SignUpRequestModel, UserEntity>()
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.CityId, opt => opt.MapFrom(
                    src => src.City))
                .ForMember(dest => dest.ProfessionId, opt => opt.MapFrom(
                    src => src.Profession))
                .ForMember(dest => dest.GenderId, opt => opt.MapFrom(
                    src => src.Gender));
        }
    }
}