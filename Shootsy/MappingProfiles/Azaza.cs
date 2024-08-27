using AutoMapper;
using Shootsy.Dtos;
using Shootsy.Models;

namespace Shootsy.MappingProfiles
{
    public class Azaza : Profile
    {
        public Azaza() =>
            CreateMap<List<UserDto>, List<UserModelResponse>>();
    }
}
