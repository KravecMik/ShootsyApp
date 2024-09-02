using AutoMapper;
using Shootsy.Database.Entities;
using Shootsy.Dtos;
using Shootsy.Models;

namespace Shootsy.MappingProfiles
{
    public class FileProfiles : Profile
    {
        public FileProfiles()
        {
            CreateMap<FileDto, FileEntity>().ReverseMap();
            CreateMap<FileDto, FileModelResponse>()
                                .ForMember(dest => dest.UserID, opt => opt.MapFrom(
                    src => src.User));
        }
    }
}
