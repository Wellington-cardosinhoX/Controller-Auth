using ApiAutoMapper.Data.DTOs.Sessao;
using System.ComponentModel.DataAnnotations;

namespace ApiAutoMapper.Data.DTOs.Filme
{
    public class ReadFilmeDto
    {
        public string Titulo { get; set; } = string.Empty;

        public string Genero { get; set; } = string.Empty;

        public int Duracao { get; set; }

        public DateTime HoraDaConsulta { get; set; } = DateTime.Now;

        public ICollection<ReadSessaoDto> Sessoes { get; set; } = null!;
    }
}
