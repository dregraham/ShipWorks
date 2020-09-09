﻿namespace Interapptive.Shared.Security
{
    /// <summary>
    /// Interface for a factory that creates Encryption Providers
    /// </summary>
    public interface IEncryptionProviderFactory
    {
        /// <summary>
        /// Creates the license encryption provider.
        /// </summary>
        /// <returns>IEncryptionProvider.</returns>
        IEncryptionProvider CreateLicenseEncryptionProvider();

        /// <summary>
        /// Creates the sears encryption provider.
        /// </summary>
        /// <returns>IEncryptionProvider.</returns>
        IEncryptionProvider CreateSearsEncryptionProvider();

        /// <summary>
        /// Creates the ODBC encryption provider.
        /// </summary>
        /// <returns>IEncryptionProvider.</returns>
        IEncryptionProvider CreateOdbcEncryptionProvider();

        /// <summary>
        /// Creates the secure text encryption provider.
        /// </summary>
        /// <param name="salt">The salt.</param>
        /// <returns>IEncryptionProvider.</returns>
        IEncryptionProvider CreateSecureTextEncryptionProvider(string salt);

        /// <summary>
        /// Creates the AES Stream encryption provider.
        /// </summary>
        /// <returns>An instance of AesStreamEncryptionProvider.</returns>
        IEncryptionProvider CreateAesStreamEncryptionProvider();

        /// <summary>
        /// Creates the walmart encryption provider.
        /// </summary>
        IEncryptionProvider CreateWalmartEncryptionProvider();

        /// <summary>
        /// Creates the ChannelAdvisor encryption provider.
        /// </summary>
        IEncryptionProvider CreateChannelAdvisorEncryptionProvider();

        /// <summary>
        /// Creates the Overstock encryption provider.
        /// </summary>
        IEncryptionProvider CreateOverstockEncryptionProvider();

        /// <summary>
        /// Creates the Rakuten encryption provider.
        /// </summary>
        IEncryptionProvider CreateRakutenEncryptionProvider();
    }
}
