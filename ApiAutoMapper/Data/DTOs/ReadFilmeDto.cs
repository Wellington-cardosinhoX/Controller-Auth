using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Data.DTOs
{
    public class ReadFilmeDto
    {
        public string Titulo { get; set; } = string.Empty;

        public string Genero { get; set; } = string.Empty;

        public int Duracao { get; set; }

        public DateTime HoraDaConsulta { get; set; } = DateTime.Now;
    }
}
