using ApiAutoMapper.Data.DTOs.Sessao;
using ApiAutoMapper.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;

namespace ApiAutoMapper.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SessaoController : ControllerBase
    {
        private FilmeContext _context;
        private readonly IMapper _mapper;

        public SessaoController(FilmeContext filmeContext, IMapper mapper)
        {
            _context = filmeContext;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> PegarSessoes([FromQuery] int skip = 0, [FromQuery] int take = 20)
        {
            var SkipTake = _context.Sessoes.Skip(skip).Take(take);

            var sessoesDto = _mapper.Map<List<ReadSessaoDto>>(SkipTake);

            if (sessoesDto is not null)
            {
                return Ok(sessoesDto);
            }

            return NotFound();
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> PegarSessaoPorId(int id)
        {
            var primeiraSessao = await _context.Sessoes.FindAsync(id);
            if (primeiraSessao is null) return NotFound($"Id {id} não encontrado!");

            var sessaoDto = _mapper.Map<ReadSessaoDto>(primeiraSessao);

            return Ok(sessaoDto);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AdicionaSessao([FromBody] CreateSessaoDto sessaoDto)
        {
            Sessao sessao = _mapper.Map<Sessao>(sessaoDto);

            var adicionaSessao = _context.Sessoes.Add(sessao);

            if (adicionaSessao is null) return NotFound();

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(PegarSessaoPorId), new { id = sessao.Id }, sessao);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarSessao(int id)
        {
            var pegarSessao = await _context.Sessoes.FindAsync(id);
            if (pegarSessao is null) return NotFound("Esse Id não existe");

            _context.Sessoes.Remove(pegarSessao);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
