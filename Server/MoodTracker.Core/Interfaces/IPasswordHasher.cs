using MoodTracker.Core.Enums;
using MoodTracker.Core.Models;

namespace MoodTracker.Core.Interfaces
{
    public interface IPasswordHasher<TUser>
        where TUser : User
    {
        string HashPassword(TUser user, string password);
        PasswordVerificationResult VerifyPasswordHash(TUser user, string passwordHash, string providedPassword);
    }
}