using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Data.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{
    private FilmeContext _filmeContext;
    private readonly IMapper _mapper;

    public FilmeController(FilmeContext filmeContext, IMapper mapper)
    {
        _filmeContext = filmeContext;
        _mapper = mapper;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> PegarFilmes([FromQuery] int skip = 0, [FromQuery] int take = 20)
    {
        var SkipTake = _filmeContext.Filmes.Skip(skip).Take(take);

        var filmesDto = _mapper.Map<List<ReadFilmeDto>>(SkipTake);

        if (filmesDto is not null) 
        {
            return Ok(filmesDto);
        }

        return NotFound();
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> PegarFilmePorId(int id)
    {
        var primeiroFilme = await _filmeContext.Filmes.FindAsync(id);
        if (primeiroFilme is null) return NotFound($"Id {id} não encontrado!");

        var filmeDto = _mapper.Map<ReadFilmeDto>(primeiroFilme);

        return Ok(filmeDto);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AdicionaFilme([FromBody] CreateFilmeDto filmeDto)
    {
        Filme filme = _mapper.Map<Filme>(filmeDto);

        var adicionaFilme = _filmeContext.Filmes.Add(filme);

        if (adicionaFilme is null) return NotFound();

        await _filmeContext.SaveChangesAsync();

        return CreatedAtAction(nameof(PegarFilmePorId), new { id = filme.Id}, filme);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarFilme(int id)
    {
        var pegarFilme = await _filmeContext.Filmes.FindAsync(id);
        if (pegarFilme is null) return NotFound("Esse Id não existe");

        _filmeContext.Filmes.Remove(pegarFilme);
        await _filmeContext.SaveChangesAsync();

        return NoContent();
    }


    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarFilme(int id, [FromBody] UpdateFilmeDto updateFilmeDto)
    {
        try
        {
            var filme = _filmeContext.Filmes.FirstOrDefault(filme => filme.Id == id);

            if (filme is null) return NotFound();

            _mapper.Map(updateFilmeDto, filme);

            await _filmeContext.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex) when (ex.Message.Contains("Missing"))
        {
            throw new Exception("Você não configurou um CreateMap<>() para o profile atual");
        }
    }


    [Authorize]
    [HttpPatch("{id}")]
    public async Task<IActionResult> AtualizarFilmeParcial(int id, JsonPatchDocument<UpdateFilmeDto> jsonPatchDocument)
    {
        var filme = await _filmeContext.Filmes.FirstOrDefaultAsync(filme => filme.Id == id);

        if (filme is null) return NotFound();
            
        var filmeParaAtualizar = _mapper.Map<UpdateFilmeDto>(filme);

        jsonPatchDocument.ApplyTo(filmeParaAtualizar, ModelState);

        if (!TryValidateModel(filmeParaAtualizar))
        {
            return ValidationProblem(ModelState);
        }

        _mapper.Map(filmeParaAtualizar, filme);

        await _filmeContext.SaveChangesAsync();

        return NoContent();   
    }
}
