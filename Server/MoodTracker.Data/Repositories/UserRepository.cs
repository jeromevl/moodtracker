using Microsoft.EntityFrameworkCore;
using MoodTracker.Core.Interfaces.Repositories;
using MoodTracker.Data.Entities;
using coreModels = MoodTracker.Core.Models;

namespace MoodTracker.Data.Repositories
{
    public class UserRepository(MoodTrackerDbContext context) : IUserRepository
    {
        private readonly MoodTrackerDbContext _context = context;

        public async Task<coreModels.User?> GetByUsernameAsync(string username)
        {
            var user = await _context.Users.Include(u => u.Role)
                .SingleOrDefaultAsync(u => u.Username == username);

            coreModels.User? mappedUser = null;
            if (user != null)
            {
                mappedUser = new coreModels.User
                {
                    Id = user.Id,
                    Username = user.Username,
                    Role = user.Role.Name
                };
            }

            return mappedUser;
        }

        public async Task<coreModels.User?> GetByAccountAsync(string username, string hashedPassword)
        {
            var user = await _context.Users.Include(u => u.Role)
                .SingleOrDefaultAsync(u => u.PasswordHash == hashedPassword);

            coreModels.User? mappedUser = null;
            if (user != null)
            {
                mappedUser = new coreModels.User
                {
                    Id = user.Id,
                    Username = user.Username,
                    Role = user.Role.Name
                };
            }

            return mappedUser;
        }

        public async Task CreateAsync(coreModels.User newUser, string? hashedPassword = null)
        {
            var role = await _context.Roles.SingleOrDefaultAsync(r => r.Name == newUser.Role);
            if (role != null)
            {
                var userToAdd = new User
                {
                    Id = newUser.Id!.Value,
                    Username = newUser.Username,
                    PasswordHash = hashedPassword,
                    RoleId = role.Id
                };

                _context.Add(userToAdd);
            }
            else
                throw new ArgumentException("Role does not exist.");

            await _context.SaveChangesAsync();
        }

        public async Task<bool> UserWithRoleExists(string roleName)
        {
            return await _context.Users.AnyAsync(u => u.Role.Name == roleName);
        }

        public async Task<string?> GetPasswordHashAsync(Guid userId)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                return user.PasswordHash;
            }
            else
                throw new ArgumentException("User does not exist.");
        }
    }
}