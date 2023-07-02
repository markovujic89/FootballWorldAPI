using Application.DTOs;
using AutoMapper;
using Domain;
using System.Diagnostics;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<PlayerDTO, Player>();
            CreateMap<Player, PlayerDTO>();
            CreateMap<ClubDTO, Club>();
            CreateMap<Club, ClubDTO>();
        }
    }
}
