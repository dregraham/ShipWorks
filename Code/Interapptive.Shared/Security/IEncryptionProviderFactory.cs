namespace Interapptive.Shared.Security
{
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
        /// Creates the secure text encryption provider.
        /// </summary>
        /// <param name="salt">The salt.</param>
        /// <returns>IEncryptionProvider.</returns>
        IEncryptionProvider CreateSecureTextEncryptionProvider(string salt);
    }
}
