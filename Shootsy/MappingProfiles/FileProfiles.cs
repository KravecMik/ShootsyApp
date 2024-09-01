using AutoMapper;
using Shootsy.Database.Entities;
using Shootsy.Dtos;

namespace Shootsy.MappingProfiles
{
    public class FileProfiles : Profile
    {
        public FileProfiles()
        {
            CreateMap<FileDto, FileEntity>();
        }
    }
}
