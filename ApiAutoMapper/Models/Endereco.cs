using System.ComponentModel.DataAnnotations;

namespace ApiAutoMapper.Models
{
    public class Endereco
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "O campo Logradouro é obrigatório.")]
        public string Logradouro { get; set; } = string.Empty;
        [Required(ErrorMessage = "O campo Número é obrigatório.")]
        public int Numero { get; set; }
    }
}
