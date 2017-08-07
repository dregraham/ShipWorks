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
        IJetToken GetToken(JetStoreEntity store);

        /// <summary>
        /// Get a token for the given username and password
        /// </summary>
        IJetToken GetToken(string username, string password);

        /// <summary>
        /// Removes the token from the Cache
        /// </summary>
        void RemoveToken(JetStoreEntity store);
    }
}