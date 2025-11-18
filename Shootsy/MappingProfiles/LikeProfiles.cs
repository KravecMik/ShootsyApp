using AutoMapper;
using Shootsy.Database.Entities;
using Shootsy.Models.Dtos;

namespace Shootsy.MappingProfiles
{
    public class LikeProfiles : Profile
    {
        public LikeProfiles()
        {
            CreateMap<LikeEntity, LikeDto>();
        }
    }
}