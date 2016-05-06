namespace Interapptive.Shared.Security
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
        /// Creates the odbc encryption provider.
        /// </summary>
        /// <returns>IEncryptionProvider.</returns>
        IEncryptionProvider CreateOdbcEncryptionProvider();

        /// <summary>
        /// Creates the secure text encryption provider.
        /// </summary>
        /// <param name="salt">The salt.</param>
        /// <returns>IEncryptionProvider.</returns>
        IEncryptionProvider CreateSecureTextEncryptionProvider(string salt);
    }
}
