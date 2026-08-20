using Microsoft.AspNetCore.Identity;
using MoodTracker.Core.Models;
using coreEnums = MoodTracker.Core.Enums;
using coreInterfaces = MoodTracker.Core.Interfaces;

namespace MoodTracker.Core.Helpers
{
    public class AspNetCoreIdentityPasswordHasher<TUser> : PasswordHasher<TUser>, coreInterfaces.IPasswordHasher<TUser>
        where TUser : User
    {
        public coreEnums.PasswordVerificationResult VerifyPasswordHash(TUser user, string passwordHash, string providedPassword)
        {
            var result = coreEnums.PasswordVerificationResult.Failed;

            PasswordVerificationResult verification = base.VerifyHashedPassword(user, passwordHash, providedPassword);
            if (verification == PasswordVerificationResult.Success || verification == PasswordVerificationResult.SuccessRehashNeeded)
                result = coreEnums.PasswordVerificationResult.Success;

            return result;
        }
    }
}
