using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Jet
{
    /// <summary>
    /// Represents a token repository
    /// </summary>
    public interface IJetTokenRepository
    {
        /// <summary>
        /// Get a token for the given store
        /// </summary>
        JetToken GetToken(JetStoreEntity store);

        /// <summary>
        /// Get a token for the given username and password
        /// </summary>
        JetToken GetToken(string username, string password);
    }
}