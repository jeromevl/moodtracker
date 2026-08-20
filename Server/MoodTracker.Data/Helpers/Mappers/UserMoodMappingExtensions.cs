using MoodTracker.Data.Entities;
using coreModels = MoodTracker.Core.Models;

namespace MoodTracker.Data.Helpers.Mappers
{
    public static class UserMoodMappingExtensions
    {
        public static coreModels.UserMood ToModel(this UserMood entity)
        {
            return new coreModels.UserMood
            {
                Username = entity.User.Username,
                Mood = new coreModels.Mood
                {
                    Id = entity.Mood.Id,
                    Name = entity.Mood.Name
                },
                Date = entity.Date,
                Remarks = entity.Remarks
            };
        }
    }
}