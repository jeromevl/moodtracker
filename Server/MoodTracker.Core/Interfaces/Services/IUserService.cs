using MoodTracker.Core.Models._Dtos;

namespace MoodTracker.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserDto?> GetUserByUsernameAsync(string username);
        Task<UserDto?> GetUserByAccountAsync(string username, string password);

        Task CreateUserAsync(UserDto user, string password);
        Task<bool> UserWithRoleExists(string roleName);
    }
}