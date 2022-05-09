using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Platform.OnlineUpdating
{
    /// <summary>
    /// Uploads shipment details to Platform
    /// </summary>
    public interface IPlatformOnlineUpdater
    {
        /// <summary>
        /// Update the online status of the given order
        /// </summary>
        Task UploadOrderShipmentDetails(StoreEntity store, IEnumerable<long> orderKeys);

        /// <summary>
        /// Uploads shipment details for a particular shipment
        /// </summary>
        Task UploadShipmentDetails(StoreEntity store, IEnumerable<long> shipmentKeys);
    }
}