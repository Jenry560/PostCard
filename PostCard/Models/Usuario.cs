using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostCard.Models
{
    public class Usuario
    {

        public int UsuarioId { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        [Required]
        public string email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;

    }
}
