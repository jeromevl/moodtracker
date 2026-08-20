using MoodTracker.Core.Models;

namespace MoodTracker.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByAccountAsync(string username, string hashedPassword);

        Task CreateAsync(User newUser, string? hashedPassword = null);
        Task<bool> UserWithRoleExists(string roleName);
        Task<string?> GetPasswordHashAsync(Guid userId);
    }
}