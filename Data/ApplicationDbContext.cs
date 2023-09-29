using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TwitterCloneAPI.Models;

namespace TwitterCloneAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure ASP.NET Core Identity
            builder.Entity<ApplicationUser>(entity =>
            {
                // Customize the user entity as needed
                entity.Property(e => e.UserName).HasMaxLength(256);
                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
                entity.Property(e => e.Email).HasMaxLength(256);
                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
                // Add additional user properties as needed
            });

            // Configure ASP.NET Core Identity roles
            builder.Entity<IdentityRole>(entity =>
            {
                // Customize the role entity as needed
                entity.Property(e => e.Name).HasMaxLength(256);
                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            // Define DbSet properties for your data models here
            builder.Entity<Tweet>();
            builder.Entity<UserProfile>();

            // Define the Follower relationship
            builder.Entity<Follower>()
                .HasKey(f => new { f.FollowerId, f.FolloweeId });

            builder.Entity<Follower>()
                .HasOne(f => f.FollowerUser)
                .WithMany()
                .HasForeignKey(f => f.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Follower>()
                .HasOne(f => f.FolloweeUser)
                .WithMany()
                .HasForeignKey(f => f.FolloweeId)
                .OnDelete(DeleteBehavior.Restrict);

        }

        // Define DbSet properties for your data models here
        public DbSet<Tweet> Tweets { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Follower> Followers { get; set; }
        // Additional DbSet properties as needed for other models
    }
}
