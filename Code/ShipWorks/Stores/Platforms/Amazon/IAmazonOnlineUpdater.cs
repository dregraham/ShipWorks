using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Amazon
{
    /// <summary>
    /// Uploads shipment details to Amazon
    /// </summary>
    public interface IAmazonOnlineUpdater
    {
        /// <summary>
        /// Update the online status of the given order
        /// </summary>
        Task UploadOrderShipmentDetails(AmazonStoreEntity store, IEnumerable<long> orderKeys);

        /// <summary>
        /// Uploads shipment details for a particular shipment
        /// </summary>
        Task UploadShipmentDetails(AmazonStoreEntity store, IEnumerable<long> shipmentKeys);
    }
}