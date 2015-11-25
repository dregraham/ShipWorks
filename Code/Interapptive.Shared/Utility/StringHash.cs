using System;
using System.Security.Cryptography;
using System.Text;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Class that implements a hash for strings.
    /// </summary>
    public class StringHash
    {
        /// <summary>
        /// Hashes the raw value into Base64 string.
        /// </summary>
        /// <param name="rawValue">The raw value.</param>
        /// <param name="salt">The salt.</param>
        /// <returns>A Base64 string of the hashed raw value.</returns>
        public string Hash(string rawValue, string salt)
        {
            using (SHA256Managed sha256 = new SHA256Managed())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(salt + rawValue));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
