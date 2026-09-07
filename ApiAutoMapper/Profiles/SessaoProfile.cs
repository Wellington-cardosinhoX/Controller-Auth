using ApiAutoMapper.Data.DTOs.Cinema;
using ApiAutoMapper.Data.DTOs.Sessao;
using ApiAutoMapper.Models;
using AutoMapper;

namespace ApiAutoMapper.Profiles
{
    public class SessaoProfile : Profile
    {
        public SessaoProfile()
        {
            CreateMap<CreateSessaoDto, Sessao>();
            CreateMap<Sessao, ReadCinemaDto>();
        }
    }
}
