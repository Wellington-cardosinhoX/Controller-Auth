using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{
    private FilmeContext _filmeContext;

    public FilmeController(FilmeContext filmeContext)
    {
        _filmeContext = filmeContext;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> PegarFilmes([FromQuery] int skip = 0, [FromQuery] int take = 20)
    {
        var SkipTake =  _filmeContext.Filmes.Skip(skip).Take(take);

        if (SkipTake is not null) 
        {
            return Ok(SkipTake);
        }

        return NotFound();
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> PegarFilmePorId(int id)
    {
        var primeiroFilme = await _filmeContext.Filmes.FindAsync(id);
        if (primeiroFilme is null) return NotFound($"Id {id} não encontrado!");

        return Ok(primeiroFilme);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AdicionaFilme([FromBody] Filme filme)
    {
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
        _filmeContext?.SaveChangesAsync();

        return NoContent();
    }
}
