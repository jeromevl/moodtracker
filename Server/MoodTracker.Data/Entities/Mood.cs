using System.ComponentModel.DataAnnotations;

namespace MoodTracker.Data.Entities
{
    public class Mood
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }
    }
}