using System.ComponentModel.DataAnnotations;

namespace ApiAutoMapper.Models
{
    public class Sessao
    {
        [Key]
        [Required]
        public int Id { get; set; }
    }
}
