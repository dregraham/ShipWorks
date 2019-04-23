using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.BigCommerce
{
    /// <summary>
    /// Allows interaction with the BigCommerce identifier
    /// </summary>
    public interface IBigCommerceIdentifier
    {
        /// <summary>
        /// Get the identifier from the given store
        /// </summary>
        string Get(BigCommerceStoreEntity store);

        /// <summary>
        /// Set the identifier on the given store
        /// </summary>
        BigCommerceStoreEntity Set(BigCommerceStoreEntity store);
    }
}
