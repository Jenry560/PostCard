using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PostCard.Models.ViewModel
{
    public class EditPostModel
    {
        public int PostId { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Menssage { get; set; }
        [Required]
        public string? Tags { get; set; }
        public string? Image { get; set; }
        public int UsuarioId { get; set; }
        [NotMapped]
        public IFormFile? File { get; set; }
    }
}