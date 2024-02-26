using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostCard.Models.ViewModel
{
    public class CreateModel
    {
        
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Menssage { get; set; }
        [Required]
        public string? Tags { get; set; }
        
        public string? Image { get; set; }
        public int UsuarioId { get; set; }

        [NotMapped]
        [Required]
        public IFormFile? File { get; set; }
    }
}
