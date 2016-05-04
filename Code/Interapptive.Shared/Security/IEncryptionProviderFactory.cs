namespace Interapptive.Shared.Security
{
    public interface IEncryptionProviderFactory
    {
        IEncryptionProvider CreateLicenseEncryptionProvider();

        IEncryptionProvider CreateSearsEncryptionProvider();

        IEncryptionProvider CreateSecureTextEncryptionProvider(string salt);
    }
}
