using AutoMapper;
using Shootsy.Database.Entities;
using Shootsy.Models.Dtos;

namespace Shootsy.MappingProfiles
{
    public class PostProfiles : Profile
    {
        public PostProfiles()
        {
            CreateMap<PostEntity, PostDto>();
        }
    }
}
