using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;

namespace ACSITPortal.Helpers
{
    public static class Encryption
    {
        /// <summary>
        /// Hashes the given password
        /// </summary>
        /// <param name="password">The password to hash</param>
        /// <returns>The hashed passsword</returns>
        public static string[] HashPassword(string password)
        {
            // Generate a 128-bit salt using a sequence of
            // cryptographically strong random bytes.
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            // return both the salted password, and the salt to store in the database
            string[] strings = new string[2];
            strings[0] = hashed;
            strings[1] = Encoding.GetEncoding(437).GetString(salt);

            return strings;
        }

        /// <summary>
        /// Hashes the given password with a specific salt
        /// </summary>
        /// <param name="password">The password to hash</param>
        /// <returns>The hashed passsword</returns>
        public static string HashPassword(string password, string s)
        {
            // break if no salt was provided
            if (s is null)
                return string.Empty;

            byte[] salt = Encoding.GetEncoding(437).GetBytes(s); // convert the salt into a byte array

            // same implementation as above
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}
