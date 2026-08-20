namespace MoodTracker.Core.Models
{
    public class User
    {
        public Guid? Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }
}