using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostCard.Models
{
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostId { get; set; }

        public string? Title { get; set; }
        public string? Menssage { get; set; }
        public string? Tags { get; set; }
        public string? Image { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuarios { get; set; } = null!;
        public List<Likes> Likes { get; set; } = new List<Likes>();

        [NotMapped]
        [Required]
        public IFormFile? File { get; set; }
    }
}
