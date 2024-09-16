﻿using AutoMapper;
using Shootsy.Database.Entities;
using Shootsy.Dtos;

namespace Shootsy.MappingProfiles
{
    public class UserSessionProfiles : Profile
    {
        public UserSessionProfiles()
        {
            CreateMap<UserSessionDto, UserSessionEntity>().ReverseMap();
        }
    }
}
