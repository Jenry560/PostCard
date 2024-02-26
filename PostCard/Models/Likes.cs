using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PostCard.Models
{
    public class Likes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Like_Id { get; set; }
        [Required]
        public int PostId { get; set; }
        public Post Posts { get; set; } = null!;
        [Required]
        public int UsuarioId { get; set; }
        public Usuario Usuarios { get; set; } = null!;
    }
}
