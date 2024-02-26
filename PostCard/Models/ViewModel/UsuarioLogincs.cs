using System.ComponentModel.DataAnnotations;

namespace PostCard.Models.ViewModel
{
    public class UsuarioLogincs
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
