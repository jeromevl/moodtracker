using MoodTracker.Core.Models;

namespace MoodTracker.Core.Interfaces.Repositories
{
    public interface IMoodRepository
    {
        Task<IEnumerable<UserMood>> GetAllByUserAsync(string username);
        Task<IEnumerable<UserMood>> GetAllUserMoodsAsync();

        Task SubmitAsync(Guid userId, int moodId, DateOnly date, string? remarks = null);
        Task<bool> ExistsAsync(Guid userId, DateOnly date);
    }
}