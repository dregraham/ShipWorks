using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using log4net;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Small utility to for decrypting \ encrypting passwords that are going to be
    /// saved locally.  Not totally secure, but better than nothing.
    /// </summary>
    public static class SecureText
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(SecureText));

        /// <summary>
        /// Decrypts a string that was returned by the Encrypt method.
        /// </summary>
        public static string Decrypt(string cipher, string salt)
        {
            if (cipher == null)
            {
                throw new ArgumentNullException("cipher");
            }

            if (salt == null)
            {
                throw new ArgumentNullException("salt");
            }

            if (cipher.Length == 0)
            {
                return string.Empty;
            }

            try
            {
                RC2CryptoServiceProvider crypto = new RC2CryptoServiceProvider();

                // Create IV from salt
                byte[] saltTotal = Encoding.UTF8.GetBytes(salt + salt.Length.ToString());
                byte[] iv = new byte[8];
                for (int i = 0; i < 8 && i < saltTotal.Length; i++)
                {
                    iv[i] = saltTotal[i];
                }

                // Dervie the key
                crypto.IV = iv;
                crypto.Key = new PasswordDeriveBytes(salt, new byte[0]).CryptDeriveKey("RC2", "MD5", 56, crypto.IV);

                byte[] encryptedBytes = Convert.FromBase64String(cipher);
                byte[] plainBytes = new Byte[1];

                MemoryStream plain = new MemoryStream();
                using (CryptoStream decoder = new CryptoStream(
                           plain,
                           crypto.CreateDecryptor(),
                           CryptoStreamMode.Write))
                {
                    decoder.Write(encryptedBytes, 0, encryptedBytes.Length);
                    decoder.FlushFinalBlock();

                    plainBytes = new byte[plain.Length];
                    plain.Position = 0;
                    plain.Read(plainBytes, 0, (int) plain.Length);
                }

                return Encoding.UTF8.GetString(plainBytes);
            }
            catch (Exception ex)
            {
                if (ex is FormatException || ex is CryptographicException)
                {
                    log.ErrorFormat("Failed to decrypt '{0}'.", cipher);
                    return string.Empty;
                }

                throw;
            }
        }

        /// <summary>
        /// Encrypts the string and returns the cipher text.
        /// </summary>
        public static string Encrypt(string value, string salt)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (salt == null)
            {
                throw new ArgumentNullException("salt");
            }

            // Create crypto provider
            RC2CryptoServiceProvider crypto = new
                RC2CryptoServiceProvider();

            // Create IV from salt
            byte[] saltTotal = Encoding.UTF8.GetBytes(salt + salt.Length.ToString());
            byte[] iv = new byte[8];
            for (int i = 0; i < 8 && i < saltTotal.Length; i++)
            {
                iv[i] = saltTotal[i];
            }

            // Dervie the key
            crypto.IV = iv;
            crypto.Key = new PasswordDeriveBytes(salt, new byte[0]).CryptDeriveKey("RC2", "MD5", 56, crypto.IV);

            byte[] encryptedBytes = new byte[1];

            MemoryStream cipher = new MemoryStream();
            using (CryptoStream encoder = new CryptoStream(
                       cipher,
                       crypto.CreateEncryptor(),
                       CryptoStreamMode.Write))
            {

                byte[] plainBytes = Encoding.UTF8.GetBytes(value);

                encoder.Write(plainBytes, 0, plainBytes.Length);
                encoder.FlushFinalBlock();

                encryptedBytes = new byte[cipher.Length];
                cipher.Position = 0;
                cipher.Read(encryptedBytes, 0, (int) cipher.Length);
            }

            return Convert.ToBase64String(encryptedBytes);
        }
    }
}
