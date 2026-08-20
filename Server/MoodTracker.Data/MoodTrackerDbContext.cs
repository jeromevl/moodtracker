using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoodTracker.Data.Entities;

namespace MoodTracker.Data
{
    public class MoodTrackerDbContext : DbContext
    {
        public MoodTrackerDbContext(DbContextOptions<MoodTrackerDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Mood> Moods { get; set; }
        public DbSet<UserMood> UserMoods { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureUser(modelBuilder.Entity<User>());
            ConfigureRole(modelBuilder.Entity<Role>());
            ConfigureMood(modelBuilder.Entity<Mood>());
            ConfigureUserMood(modelBuilder.Entity<UserMood>());
        }
        //
        // Helper methods
        //
        private void ConfigureUser(EntityTypeBuilder<User> entityBuilder)
        {
            entityBuilder.HasIndex(e => new { e.Username, e.DeletedAt })
                .IsUnique()
                .HasFilter(null);

            entityBuilder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entityBuilder.HasQueryFilter(e => e.DeletedAt == null);
        }

        private void ConfigureRole(EntityTypeBuilder<Role> entityBuilder)
        {
            entityBuilder.HasIndex(e => new { e.Name })
                .IsUnique()
                .HasFilter(null);

            entityBuilder.HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "User" });
        }

        private void ConfigureMood(EntityTypeBuilder<Mood> entityBuilder)
        {
            entityBuilder.HasIndex(e => new { e.Name })
                .IsUnique();

            entityBuilder.HasData(
                new Mood { Id = 1, Name = "Not good at all" },
                new Mood { Id = 2, Name = "A bit \"meh\"" },
                new Mood { Id = 3, Name = "Pretty good" },
                new Mood { Id = 4, Name = "Feeling great" });
        }

        private void ConfigureUserMood(EntityTypeBuilder<UserMood> entityBuilder)
        {
            entityBuilder.HasIndex(e => new { e.UserId, e.Date })
                .IsUnique();
        }
    }
}