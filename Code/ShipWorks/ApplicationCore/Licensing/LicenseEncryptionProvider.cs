using System;
using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Class for encrypting and decrypting license keys
    /// </summary>
    public class LicenseEncryptionProvider : IEncryptionProvider
    {
        private readonly IDatabaseIdentifier databaseID;

        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseEncryptionProvider"/> class.
        /// </summary>
        /// <param name="databaseID">The database identifier.</param>
        public LicenseEncryptionProvider(IDatabaseIdentifier databaseID)
        {
            this.databaseID = databaseID;
        }

        /// <summary>
        /// Encrypts the given decrypted text.
        /// </summary>
        public string Encrypt(string decryptedText)
        {
            return SecureText.Encrypt(decryptedText, databaseID.Get().ToString().ToUpperInvariant());
        }

        /// <summary>
        /// Decrypts the given encrypted text.
        /// </summary>
        public string Decrypt(string encryptedText)
        {
            if (string.IsNullOrWhiteSpace(encryptedText))
            {
                throw new ArgumentException("Cannot decrypt an empty string");
            }

            return SecureText.Decrypt(encryptedText, databaseID.Get().ToString().ToUpperInvariant());
        }
    }
}