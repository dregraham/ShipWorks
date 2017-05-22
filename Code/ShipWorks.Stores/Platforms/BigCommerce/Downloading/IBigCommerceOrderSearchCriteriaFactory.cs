using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.BigCommerce.Enums;

namespace ShipWorks.Stores.Platforms.BigCommerce.Downloading
{
    /// <summary>
    /// Factory for creating BigCommerce order search criteria
    /// </summary>
    public interface IBigCommerceOrderSearchCriteriaFactory
    {
        /// <summary>
        /// Gets the last online modified date from the orders table, and adds 1 second so that we don't processes the already downloaded
        /// order multiple times.
        /// </summary>
        /// <returns>BigCommerceWebClientOrderSearchCriteria </returns>
        BigCommerceWebClientOrderSearchCriteria Create(IStoreEntity store, BigCommerceWebClientOrderDateSearchType orderDateSearchType);
    }
}