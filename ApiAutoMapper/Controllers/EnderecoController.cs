using ApiAutoMapper.Data.DTOs.Cinema;
using ApiAutoMapper.Data.DTOs.Endereco;
using ApiAutoMapper.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiAutoMapper.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EnderecoController : ControllerBase
    {
        private FilmeContext _context;
        private readonly IMapper _mapper;

        public EnderecoController(FilmeContext filmeContext, IMapper mapper)
        {
            _context = filmeContext;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> PegarEnderecos([FromQuery] int skip = 0, [FromQuery] int take = 20)
        {
            var SkipTake = _context.Enderecos.Skip(skip).Take(take);

            var enderecosDto = _mapper.Map<List<ReadEnderecoDto>>(SkipTake);

            if (enderecosDto is not null)
            {
                return Ok(enderecosDto);
            }

            return NotFound();
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> PegarEnderecoPorId(int id)
        {
            var primeiroEndereco = await _context.Enderecos.FindAsync(id);
            if (primeiroEndereco is null) return NotFound($"Id {id} não encontrado!");

            var enderecoDto = _mapper.Map<ReadEnderecoDto>(primeiroEndereco);

            return Ok(enderecoDto);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AdicionaEndereco([FromBody] CreateEnderecoDto enderecoDto)
        {
            Endereco endereco = _mapper.Map<Endereco>(enderecoDto);

            var adicionaEndereco = _context.Enderecos.Add(endereco);

            if (adicionaEndereco is null) return NotFound();

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(PegarEnderecoPorId), new { id = endereco.Id }, endereco);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarEndereco(int id)
        {
            var pegarEndereco = await _context.Enderecos.FindAsync(id);
            if (pegarEndereco is null) return NotFound("Esse Id não existe");

            _context.Enderecos.Remove(pegarEndereco);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarEndereco(int id, [FromBody] UpdateEnderecoDto updateEnderecoDto)
        {
            try
            {
                var endereco = _context.Enderecos.FirstOrDefault(endereco => endereco.Id == id);

                if (endereco is null) return NotFound();

                _mapper.Map(updateEnderecoDto, endereco);

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
        public async Task<IActionResult> AtualizarEnderecoParcial(int id, JsonPatchDocument<UpdateEnderecoDto> jsonPatchDocument)
        {
            var endereco = await _context.Enderecos.FirstOrDefaultAsync(endereco => endereco.Id == id);

            if (endereco is null) return NotFound();

            var enderecoParaAtualizar = _mapper.Map<UpdateEnderecoDto>(endereco);

            jsonPatchDocument.ApplyTo(enderecoParaAtualizar, ModelState);

            if (!TryValidateModel(enderecoParaAtualizar))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(enderecoParaAtualizar, endereco);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
