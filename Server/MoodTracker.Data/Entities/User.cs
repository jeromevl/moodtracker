using System.ComponentModel.DataAnnotations.Schema;

namespace MoodTracker.Data.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string? PasswordHash { get; set; }

        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? DeletedAt { get; set; }
    }
}