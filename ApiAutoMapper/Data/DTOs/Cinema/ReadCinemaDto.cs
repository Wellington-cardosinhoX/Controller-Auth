using ApiAutoMapper.Data.DTOs.Endereco;

namespace ApiAutoMapper.Data.DTOs.Cinema
{
    public class ReadCinemaDto
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public ReadEnderecoDto ReadEnderecoDto { get; set; } = null!;

        public ICollection<ApiAutoMapper.Models.Sessao> Sessoes { get; set; } = null!;
    }
}
