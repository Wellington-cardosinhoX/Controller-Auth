using AutoMapper;
using WebApplication1.Data.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Profiles
{
    public class FilmeProfile : Profile
    {
        public FilmeProfile()
        {
            CreateMap<CreateFilmeDto, Filme>();
            CreateMap<UpdateFilmeDto, Filme>();
            CreateMap<Filme, UpdateFilmeDto>();
        }
    }
}
