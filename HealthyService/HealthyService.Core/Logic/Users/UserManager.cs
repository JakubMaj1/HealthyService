using HealthyService.Core.Database.Tables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HealthyService.Core.Logic.Users
{
    public class UserManager
    {
        public async Task<User> GetUserAsync(string emailOrLogin)
        {
            using (var dbContext = new HealthyService.Core.Database.HealthyServiceContext())
            {
                var User = await dbContext.Users.Where(q => q.IsActive && !q.IsDeleted)
                    .Where(q => EF.Functions.Like(q.Email, emailOrLogin) || EF.Functions.Like(q.Login, emailOrLogin)).FirstOrDefaultAsync();

                return User;
            }
        }

        public string EncodePassword(string email, string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(email.ToLowerInvariant() + password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            string savedPasswordHash = Convert.ToBase64String(hashBytes);
            return savedPasswordHash;
        }
        public bool CheckPassword(string email, string savedPasswordHash, string passwordToCheck)
        {
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);

            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            var pbkdf2 = new Rfc2898DeriveBytes(email.ToLowerInvariant() + passwordToCheck, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            bool result = true;
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    result = false;
                    break;
                }
            }

            return result;
        }
    }
}
