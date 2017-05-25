namespace Interapptive.Shared.Security
{
    /// <summary>
    /// Encryption provider that is salted using the DB GUID
    /// </summary>
    /// <remarks>
    /// The purpose of this interface is so that classes can take a dependency on a specific encryption
    /// provider without needing to go through the factory. This makes tests simpler and the consuming
    /// code a bit easier to read and write.
    /// </remarks>
    public interface IDatabaseSpecificEncryptionProvider : IEncryptionProvider
    {

    }
}