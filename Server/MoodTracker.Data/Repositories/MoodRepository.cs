using Microsoft.EntityFrameworkCore;
using MoodTracker.Core.Interfaces.Repositories;
using MoodTracker.Data.Entities;
using MoodTracker.Data.Helpers.Mappers;
using coreModels = MoodTracker.Core.Models;

namespace MoodTracker.Data.Repositories
{
    public class MoodRepository(MoodTrackerDbContext context) : IMoodRepository
    {
        private readonly MoodTrackerDbContext _context = context;

        public async Task<IEnumerable<coreModels.UserMood>> GetAllByUserAsync(string username)
        {
            var userMoods = await _context.UserMoods.Include(um => um.User).Include(um => um.Mood)
                .Where(um => um.User.Username == username)
                .OrderByDescending(um => um.Date).ThenByDescending(um => um.Id)
                .ToListAsync();

            var mappedUserMoods = new List<coreModels.UserMood>();
            foreach (var userMood in userMoods)
                mappedUserMoods.Add(userMood.ToModel());

            return mappedUserMoods;
        }

        public async Task<IEnumerable<coreModels.UserMood>> GetAllUserMoodsAsync()
        {
            var userMoods = await _context.UserMoods.Include(um => um.User).Include(um => um.Mood)
                .OrderByDescending(um => um.Date).ThenByDescending(um => um.Id)
                .ToListAsync();

            var mappedUserMoods = new List<coreModels.UserMood>();
            foreach (var userMood in userMoods)
                mappedUserMoods.Add(userMood.ToModel());

            return mappedUserMoods;
        }

        public async Task SubmitAsync(Guid userId, int moodId, DateOnly date, string? remarks = null)
        {
            var newUserMood = new UserMood
            {
                UserId = userId,
                MoodId = moodId,
                Date = date,
                Remarks = remarks
            };

            _context.Add(newUserMood);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid userId, DateOnly date)
        {
            return await _context.UserMoods
                .AnyAsync(um => um.UserId == userId && um.Date == date);
        }
    }
}