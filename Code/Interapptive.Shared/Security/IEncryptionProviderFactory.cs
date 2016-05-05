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
        IEncryptionProvider CreateLicenseEncryptionProvider();

        /// <summary>
        /// Creates the sears encryption provider.
        /// </summary>
        IEncryptionProvider CreateSearsEncryptionProvider();

        /// <summary>
        /// Creates the secure text encryption provider.
        /// </summary>
        IEncryptionProvider CreateSecureTextEncryptionProvider(string salt);
    }
}
