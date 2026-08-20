using MoodTracker.Core.Interfaces.Repositories;
using MoodTracker.Core.Interfaces.Services;
using MoodTracker.Core.Models._Dtos;

namespace MoodTracker.Core.Services
{
    public class MoodService(IMoodRepository moodRepository,
        IUserRepository userRepository) : IMoodService
    {
        private readonly IMoodRepository _moodRepository = moodRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<IEnumerable<UserMoodDto>> GetMoodsByUserAsync(string username)
        {
            var userMoods = await _moodRepository.GetAllByUserAsync(username);

            var mappedUserMoods = new List<UserMoodDto>();
            foreach (var userMood in userMoods)
                mappedUserMoods.Add(UserMoodDto.FromModel(userMood));

            return mappedUserMoods;
        }

        public async Task<IEnumerable<UserMoodDto>> GetUserMoodsAsync()
        {
            var userMoods = await _moodRepository.GetAllUserMoodsAsync();

            var mappedUserMoods = new List<UserMoodDto>();
            foreach (var userMood in userMoods)
                mappedUserMoods.Add(UserMoodDto.FromModel(userMood));

            return mappedUserMoods;
        }

        public async Task SubmitMoodByUsernameAsync(string username, int moodId, DateOnly? date = null, string? remarks = null)
        {
            DateOnly? dateToUse = date;
            if (!dateToUse.HasValue)
                dateToUse = DateOnly.FromDateTime(DateTime.UtcNow);

            // Get or create user
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null)
            {
                user = new Models.User
                {
                    Id = Guid.NewGuid(),
                    Username = username,
                    Role = "User"
                };

                await _userRepository.CreateAsync(user);
            }

            // Try create mood
            var exists = await _moodRepository.ExistsAsync(user.Id!.Value, dateToUse.Value);
            if (!exists)
                await _moodRepository.SubmitAsync(user.Id.Value, moodId, dateToUse.Value, remarks);
            else
                throw new InvalidOperationException("Mood entry for the specified user and date already exists.");
        }
    }
}