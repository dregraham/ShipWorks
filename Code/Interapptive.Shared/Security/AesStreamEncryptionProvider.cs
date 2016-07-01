using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Interapptive.Shared.Utility;

namespace Interapptive.Shared.Security
{
    /// <summary>
    /// IEncryptionProvider for encrypting and decrypting files (streams in this case)
    /// http://stackoverflow.com/questions/27645527/aes-encryption-on-large-files
    /// </summary>
    public class AesStreamEncryptionProvider : IEncryptionProvider
    {
        private readonly ICipherKey cipherKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="AesStreamEncryptionProvider"/> class.
        /// </summary>
        public AesStreamEncryptionProvider(ICipherKey cipherKey)
        {
            this.cipherKey = cipherKey;
        }

        /// <summary>
        /// Encrypt an input stream, saving to an output stream using RijndaelManaged encryption.
        /// </summary>
        public void Encrypt(Stream sourceStream, Stream outputStream)
        {
            MethodConditions.EnsureArgumentIsNotNull(sourceStream);
            MethodConditions.EnsureArgumentIsNotNull(outputStream);

            //Set Rijndael symmetric encryption algorithm
            using (RijndaelManaged rijndaelManaged = GetRijndaelManaged())
            {
                using (Rfc2898DeriveBytes key = GetRijndaeKey(rijndaelManaged))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(outputStream, rijndaelManaged.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        WriteToStream(sourceStream, cryptoStream);
                    }
                }
            }
        }

        /// <summary>
        /// Decrypt an input stream, saving to an output stream using RijndaelManaged encryption.
        /// </summary>
        public void Decrypt(Stream sourceStream, Stream outputStream)
        {
            MethodConditions.EnsureArgumentIsNotNull(sourceStream);
            MethodConditions.EnsureArgumentIsNotNull(outputStream);

            //Set Rijndael symmetric encryption algorithm
            using (RijndaelManaged rijndaelManaged = GetRijndaelManaged())
            {
                using (Rfc2898DeriveBytes key = GetRijndaeKey(rijndaelManaged))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(sourceStream, rijndaelManaged.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        WriteToStream(cryptoStream, outputStream);
                    }
                }
            }
        }

        /// <summary>
        /// Get a new RijndaelManaged with appropriate settings
        /// </summary>
        private RijndaelManaged GetRijndaelManaged()
        {
            //Set Rijndael symmetric encryption algorithm
            RijndaelManaged rijndaelManaged = new RijndaelManaged
            {
                KeySize = 256,
                BlockSize = 128,
                Padding = PaddingMode.PKCS7,

                //Cipher modes: http://security.stackexchange.com/questions/52665/which-is-the-best-cipher-mode-and-padding-mode-for-aes-encryption
                Mode = CipherMode.CFB
            };

            return rijndaelManaged;
        }

        /// <summary>
        /// Get a new key (Rfc2898DeriveBytes) with appropriate settings for the given RijndaelManaged object
        /// </summary>
        private Rfc2898DeriveBytes GetRijndaeKey(RijndaelManaged rijndaelManaged)
        {
            //http://stackoverflow.com/questions/2659214/why-do-i-need-to-use-the-rfc2898derivebytes-class-in-net-instead-of-directly
            //"What it does is repeatedly hash the user password along with the salt." High iteration counts.
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(cipherKey.Key, cipherKey.InitializationVector, 50000);

            rijndaelManaged.Key = key.GetBytes(rijndaelManaged.KeySize/8);
            rijndaelManaged.IV = key.GetBytes(rijndaelManaged.BlockSize/8);

            return key;
        }

        /// <summary>
        /// Write data from sourceStream to outputStream
        /// </summary>
        private static void WriteToStream(Stream sourceStream, Stream outputStream)
        {
            //create a buffer (1mb) so only this amount will allocate in the memory and not the whole stream
            byte[] buffer = new byte[1048576];
            int read;

            while ((read = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                outputStream.Write(buffer, 0, read);
            }
        }

        /// <summary>
        /// Encrypts the given decrypted text.
        /// NOT implemented
        /// </summary>
        public string Encrypt(string plainText)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Decrypts the given encrypted text.
        /// NOT implemented
        /// </summary>
        public string Decrypt(string encryptedText)
        {
            throw new NotImplementedException();
        }
    }
}
