using System.ComponentModel.DataAnnotations;

namespace ApiAutoMapper.Data.DTOs.Cinema
{
    public class CreateCinemaDto
    {
        [Required(ErrorMessage = "O campo de nome é obrigatório")]
        public string Nome { get; set; } = string.Empty;
        public int EnderecoId { get; set; }
    }
}
