namespace MoodTracker.Core.Models
{
    public class UserMood
    {
        public string Username { get; set; }
        public Mood Mood { get; set; }
        public DateOnly? Date { get; set; }
        public string? Remarks { get; set; }
    }
}