using ApiAutoMapper.Models;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class Filme
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "O título não pode ser vazio")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "O gênero não pode ser vazio")]
    public string Genero { get; set; } = string.Empty;

    [Required(ErrorMessage = "A duração não pode ser vazio")]
    [Range(70, int.MaxValue, ErrorMessage = "A duração deve ser apartir de 70 minutos")]
    public int Duracao { get; set; }

    public virtual ICollection<Sessao> Sessoes { get; set; } = null!;
}
