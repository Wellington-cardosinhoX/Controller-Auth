using ApiAutoMapper.Data.DTOs.Cinema;
using ApiAutoMapper.Models;
using AutoMapper;

namespace ApiAutoMapper.Profiles;

public class CinemaProfile : Profile
{
    public CinemaProfile()
    {
        CreateMap<CreateCinemaDto, Cinema>();
        CreateMap<Cinema, ReadCinemaDto>()
            .ForMember(cinemaDto => cinemaDto.ReadEnderecoDto,
                opt => opt.MapFrom(cinema => cinema.Endereco));
        CreateMap<UpdateCinemaDto, Cinema>();
    }
}
