using AutoMapper;
using Shootsy.Database.Entities;
using Shootsy.Models.Dtos;

namespace Shootsy.MappingProfiles
{
    public class CommentProfiles : Profile
    {
        public CommentProfiles()
        {
            CreateMap<CommentEntity, CommentDto>();
        }
    }
}