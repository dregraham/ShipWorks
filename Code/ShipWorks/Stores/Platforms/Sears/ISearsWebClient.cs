using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.Sears.OnlineUpdating;

namespace ShipWorks.Stores.Platforms.Sears
{
    /// <summary>
    /// Used to interact with a sears.com store
    /// </summary>
    public interface ISearsWebClient
    {
        /// <summary>
        /// Return the next page of orders based on the given lastModified date
        /// </summary>
        SearsOrdersPage GetNextOrdersPage(ISearsStoreEntity store);

        /// <summary>
        /// Initialize for downloading
        /// </summary>
        void InitializeForDownload(DateTime startDate);

        /// <summary>
        /// Test to see if the credentials for the current store are valid
        /// </summary>
        void TestConnection(ISearsStoreEntity store);

        /// <summary>
        /// Upload the details of the given shipment to Sears
        /// </summary>
        void UploadShipmentDetails(ISearsStoreEntity store, SearsOrderDetail orderDetail, IEnumerable<SearsTracking> searsTrackingEntries);
    }
}