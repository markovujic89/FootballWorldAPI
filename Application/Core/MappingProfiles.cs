using Application.DTOs;
using AutoMapper;
using Domain;

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
            CreateMap<Club, CreateClubDTO>();
            CreateMap<CreateClubDTO, Club>();
            CreateMap<Player, CreatePlayerDTO>();
            CreateMap<CreatePlayerDTO, Player>();
            CreateMap<Club, EditClubDTO>();
            CreateMap<EditClubDTO, Club>();
            CreateMap<Player, EditPlayerDTO>();
            CreateMap<EditPlayerDTO, Player>();
        }
    }
}
