using Microsoft.EntityFrameworkCore;

namespace PostCard.Models
{
    public class AplicationContext : DbContext
    {
        public AplicationContext(DbContextOptions<AplicationContext> option): base(option)
        {
            
        }

        public DbSet<Usuario> Usuarios { get; set; } = null!;
        public DbSet<Post> Posts { get; set; }= null!;
        public DbSet<Likes> Likes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .HasMany(u => u.Likes)
                .WithOne(l => l.Posts)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.Cascade);

                
        }


    }
}
