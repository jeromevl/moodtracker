using MoodTracker.Core.Enums;
using MoodTracker.Core.Interfaces;
using MoodTracker.Core.Interfaces.Repositories;
using MoodTracker.Core.Interfaces.Services;
using MoodTracker.Core.Models;
using MoodTracker.Core.Models._Dtos;

namespace MoodTracker.Core.Services
{
    public class UserService(IUserRepository userRepository,
        IPasswordHasher<User> passwordHasher) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;

        public async Task<UserDto?> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);

            UserDto? mapperUser = null;
            if (user != null)
            {
                mapperUser = UserDto.FromModel(user);
            }

            return mapperUser;
        }

        public async Task<UserDto?> GetUserByAccountAsync(string username, string password)
        {
            UserDto? mapperUser = null;

            var user = await _userRepository.GetByUsernameAsync(username);
            if (user != null)
            {
                var passwordHash = await _userRepository.GetPasswordHashAsync(user.Id!.Value);
                if (passwordHash == null)
                    throw new InvalidOperationException("Invalid username and password combination.");

                var passwordResult = _passwordHasher.VerifyPasswordHash(user, passwordHash, password);
                if (passwordResult == PasswordVerificationResult.Success)
                {
                    mapperUser = UserDto.FromModel(user);
                }
            }

            return mapperUser;
        }

        public async Task CreateUserAsync(UserDto user, string password)
        {
            var newUserToAdd = user.ToModel();

            string? hashedPassword = null;
            if (!String.IsNullOrWhiteSpace(password))
                hashedPassword = _passwordHasher.HashPassword(newUserToAdd, password);

            await _userRepository.CreateAsync(newUserToAdd, hashedPassword);
        }

        public async Task<bool> UserWithRoleExists(string roleName)
        {
            return await _userRepository.UserWithRoleExists(roleName);
        }
    }
}