namespace MoodTracker.Core.Models._Dtos
{
    public class UserMoodDto
    {
        public string Username { get; set; }
        public MoodDto Mood { get; set; }
        public DateOnly? Date { get; set; }
        public string? Remarks { get; set; }

        public UserMood ToModel()
        {
            return new UserMood
            {
                Username = this.Username,
                Mood = new Mood
                {
                    Id = this.Mood.Id,
                    Name = this.Mood.Name
                },
                Date = this.Date,
                Remarks = this.Remarks
            };
        }

        public static UserMoodDto FromModel(UserMood model)
        {
            return new UserMoodDto
            {
                Username = model.Username,
                Mood = new MoodDto
                {
                    Id = model.Mood.Id,
                    Name = model.Mood.Name
                },
                Date = model.Date,
                Remarks = model.Remarks
            };
        }
    }
}