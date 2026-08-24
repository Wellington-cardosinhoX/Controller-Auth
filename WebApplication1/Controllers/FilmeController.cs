using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{
    private static List<Filme> filmes = [];
    private static int id = 0;

    [Authorize]
    [HttpGet]
    public IActionResult PegarFilmes([FromQuery] int skip = 0, [FromQuery] int take = 20)
    {
        var SkipTake = filmes.Skip(skip).Take(take);

        if (SkipTake is not null) 
        {
            return Ok(SkipTake);
        }

        return NotFound();
    }

    [Authorize]
    [HttpGet("{id}")]
    public IActionResult PegarFilmePorId(int id)
    {
        var primeiroFilme = filmes.FirstOrDefault(t => t.Id == id);
        if (primeiroFilme is null) return NotFound($"Id {id} não encontrado!");

        return Ok(primeiroFilme);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult AdicionaFilme([FromBody] Filme filme)
    {
        filme.Id = id++;
        filmes.Add(filme);
        return CreatedAtAction(nameof(PegarFilmePorId), new { id = filme.Id}, filme);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public IActionResult DeletarFilme(int id)
    {
        var pegarFilme = filmes.FirstOrDefault(t => t.Id == id);
        if (pegarFilme is null) return NotFound("Esse Id não existe");

        filmes.Remove(pegarFilme);

        return Ok(pegarFilme);
    }
}
