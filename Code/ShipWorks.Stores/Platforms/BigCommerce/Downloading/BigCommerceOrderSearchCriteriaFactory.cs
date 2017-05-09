using System;
using Interapptive.Shared.Utility;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.BigCommerce.Enums;

namespace ShipWorks.Stores.Platforms.BigCommerce.Downloading
{
    /// <summary>
    /// Factory for creating BigCommerce order search criteria
    /// </summary>
    [Component]
    public class BigCommerceOrderSearchCriteriaFactory : IBigCommerceOrderSearchCriteriaFactory
    {
        readonly IDownloadStartingPoint startingPoint;
        readonly IDateTimeProvider dateTimeProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceOrderSearchCriteriaFactory(IDownloadStartingPoint startingPoint, IDateTimeProvider dateTimeProvider)
        {
            this.dateTimeProvider = dateTimeProvider;
            this.startingPoint = startingPoint;
        }

        /// <summary>
        /// Gets the last online modified date from the orders table, and adds 1 second so that we don't processes the already downloaded
        /// order multiple times.
        /// </summary>
        /// <returns>BigCommerceWebClientOrderSearchCriteria </returns>
        public BigCommerceWebClientOrderSearchCriteria Create(IStoreEntity store, BigCommerceWebClientOrderDateSearchType searchType)
        {
            // Getting last online modified starting point
            DateTime? createdDateStartingPoint = startingPoint.OrderDate(store);

            // If the date has a value, add 1 second, otherwise default to 6 months back
            createdDateStartingPoint = createdDateStartingPoint.HasValue ?
                createdDateStartingPoint.Value.ToUniversalTime() :
                dateTimeProvider.UtcNow.AddMonths(-6);

            // Set end date to now
            DateTime createdDateEndPoint = dateTimeProvider.UtcNow;

            // Getting last online modified starting point
            DateTime? modifiedDateStartingPoint = startingPoint.OnlineLastModified(store);

            // If the date has a value, add 1 second, otherwise default to 6 months back
            modifiedDateStartingPoint = modifiedDateStartingPoint.HasValue ?
                modifiedDateStartingPoint.Value.ToUniversalTime() :
                dateTimeProvider.UtcNow.AddMonths(-6);

            // Set end date to now
            DateTime modifiedDateEndPoint = dateTimeProvider.UtcNow;

            BigCommerceWebClientOrderSearchCriteria orderSearchCriteria =
                new BigCommerceWebClientOrderSearchCriteria(searchType,
                    modifiedDateStartingPoint.Value, modifiedDateEndPoint,
                    createdDateStartingPoint.Value, createdDateEndPoint);

            // Create the order search criteria
            return orderSearchCriteria;
        }
    }
}
