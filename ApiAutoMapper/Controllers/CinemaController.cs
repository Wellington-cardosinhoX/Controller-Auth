using ApiAutoMapper.Data.DTOs.Cinema;
using ApiAutoMapper.Data.DTOs.Filme;
using ApiAutoMapper.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace ApiAutoMapper.Controllers;

public class CinemaController : ControllerBase
{
    private FilmeContext _context;
    private readonly IMapper _mapper;

    public CinemaController(FilmeContext filmeContext, IMapper mapper)
    {
        _context = filmeContext;
        _mapper = mapper;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> PegarCinemas([FromQuery] int skip = 0, [FromQuery] int take = 20)
    {
        var SkipTake = _context.Cinemas.Skip(skip).Take(take);

        var cinemasDto = _mapper.Map<List<ReadCinemaDto>>(SkipTake);

        if (cinemasDto is not null)
        {
            return Ok(cinemasDto);
        }

        return NotFound();
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> PegarCinemaPorId(int id)
    {
        var primeiroCinema = await _context.Filmes.FindAsync(id);
        if (primeiroCinema is null) return NotFound($"Id {id} não encontrado!");

        var filmeDto = _mapper.Map<ReadCinemaDto>(primeiroCinema);

        return Ok(filmeDto);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AdicionaCinema([FromBody] CreateCinemaDto cinemaDto)
    {
        Cinema cinema = _mapper.Map<Cinema>(cinemaDto);

        var adicionaCinema = _context.Cinemas.Add(cinema);

        if (adicionaCinema is null) return NotFound();

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(PegarCinemaPorId), new { id = cinema.Id }, cinema);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarCinema(int id)
    {
        var pegarCinema = await _context.Cinemas.FindAsync(id);
        if (pegarCinema is null) return NotFound("Esse Id não existe");

        _context.Cinemas.Remove(pegarCinema);
        await _context.SaveChangesAsync();

        return NoContent();
    }


    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarCinema(int id, [FromBody] UpdateCinemaDto updateFilmeDto)
    {
        try
        {
            var cinema = _context.Cinemas.FirstOrDefault(cinema => cinema.Id == id);

            if (cinema is null) return NotFound();

            _mapper.Map(updateFilmeDto, cinema);

            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex) when (ex.Message.Contains("Missing"))
        {
            throw new Exception("Você não configurou um CreateMap<>() para o profile atual");
        }
    }


    [Authorize]
    [HttpPatch("{id}")]
    public async Task<IActionResult> AtualizarCinemaParcial(int id, JsonPatchDocument<UpdateCinemaDto> jsonPatchDocument)
    {
        var cinema = await _context.Cinemas.FirstOrDefaultAsync(cinema => cinema.Id == id);

        if (cinema is null) return NotFound();

        var cinemaParaAtualizar = _mapper.Map<UpdateCinemaDto>(cinema);

        jsonPatchDocument.ApplyTo(cinemaParaAtualizar, ModelState);

        if (!TryValidateModel(cinemaParaAtualizar))
        {
            return ValidationProblem(ModelState);
        }

        _mapper.Map(cinemaParaAtualizar, cinema);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}
