using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoodTracker.Data.Entities
{
    public class UserMood
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public int MoodId { get; set; }

        [ForeignKey("MoodId")]
        public Mood Mood { get; set; }

        public DateOnly Date { get; set; }

        [StringLength(250)]
        public string? Remarks { get; set; }
    }
}