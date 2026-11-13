using ApiAutoMapper.Data.DTOs.Endereco;
using ApiAutoMapper.Data.DTOs.Sessao;

namespace ApiAutoMapper.Data.DTOs.Cinema
{
    public class ReadCinemaDto
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public ReadEnderecoDto ReadEnderecoDto { get; set; } = null!;

        public ICollection<ReadSessaoDto> Sessoes { get; set; } = null!;
    }
}
