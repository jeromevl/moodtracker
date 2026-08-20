using MoodTracker.Core.Models;
using MoodTracker.Core.Models._Dtos;

namespace MoodTracker.Core.Interfaces.Services
{
    public interface IMoodService
    {
        Task<IEnumerable<UserMoodDto>> GetMoodsByUserAsync(string username);
        Task<IEnumerable<UserMoodDto>> GetUserMoodsAsync();

        Task SubmitMoodByUsernameAsync(string username, int moodId, DateOnly? date = null, string? remarks = null);
    }
}