using EatClean.MobileAppService.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EatClean.MobileAppService.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Story> Stories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder) 
        {
            builder.Entity<Story>()
                .HasOne(s => s.User)
                .WithMany(u => u.Stories)                
                .HasForeignKey(s => s.UserId)                
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Stories_Users_UserId");

            builder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Comments_Users_UserId");

            builder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Likes_Users_UserId");

            builder.Entity<Comment>()
                .HasOne(c => c.Story)
                .WithMany(s => s.Comments)
                .HasForeignKey(c => c.StoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Comments_Stories_StoryId");

            builder.Entity<Like>()
                .HasOne(l => l.Story)
                .WithMany(s => s.Likes)
                .HasForeignKey(l => l.StoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Likes_Stories_StoryId");

            base.OnModelCreating(builder);
        }
    }
}