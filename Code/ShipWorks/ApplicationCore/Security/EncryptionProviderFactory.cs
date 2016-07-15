﻿using Autofac.Features.Indexed;
using Interapptive.Shared.Security;
using ShipWorks.Data.Administration;

namespace ShipWorks.ApplicationCore.Security
{
    public class EncryptionProviderFactory : IEncryptionProviderFactory
    {
        private readonly IIndex<CipherContext, ICipherKey> cipherKeyFactory;
        private readonly ISqlSchemaVersion sqlSchemaVersion;

        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptionProviderFactory" /> class.
        /// </summary>
        /// <param name="cipherKeyFactory">The cipher key factory.</param>
        /// <param name="sqlSchemaVersion">The SQL schema version.</param>
        public EncryptionProviderFactory(IIndex<CipherContext, ICipherKey> cipherKeyFactory, ISqlSchemaVersion sqlSchemaVersion)
        {
            this.cipherKeyFactory = cipherKeyFactory;
            this.sqlSchemaVersion = sqlSchemaVersion;
        }

        /// <summary>
        /// Creates the license encryption provider.
        /// </summary>
        /// <returns>An instance of LicenseEncryptionProvider.</returns>
        public IEncryptionProvider CreateLicenseEncryptionProvider()
        {
            ICipherKey cipherKey = cipherKeyFactory[CipherContext.License];
            return new LicenseEncryptionProvider(cipherKey, sqlSchemaVersion);
        }

        /// <summary>
        /// Creates the Sears encryption provider.
        /// </summary>
        /// <returns>An instance of AesEncryptionProvider.</returns>
        public IEncryptionProvider CreateSearsEncryptionProvider()
        {
            ICipherKey cipherKey = cipherKeyFactory[CipherContext.Sears];
            return new AesEncryptionProvider(cipherKey);
        }

        /// <summary>
        /// Creates the secure text encryption provider.
        /// </summary>
        /// <param name="salt">The salt.</param>
        /// <returns>An instance of SecureTextEncryptionProvider.</returns>
        public IEncryptionProvider CreateSecureTextEncryptionProvider(string salt)
        {
            return new SecureTextEncryptionProvider(salt);
        }

        /// <summary>
        /// Creates the AES Stream encryption provider.
        /// </summary>
        /// <returns>An instance of AesStreamEncryptionProvider.</returns>
        public IEncryptionProvider CreateAesStreamEncryptionProvider()
        {
            ICipherKey cipherKey = cipherKeyFactory[CipherContext.Stream];
            return new AesStreamEncryptionProvider(cipherKey);
        }
    }
}