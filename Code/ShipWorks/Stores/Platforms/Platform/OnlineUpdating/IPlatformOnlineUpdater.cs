using System.Collections.Generic;
using System.Threading.Tasks;

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
        Task UploadOrderShipmentDetails(IEnumerable<long> orderKeys);

        /// <summary>
        /// Uploads shipment details for a particular shipment
        /// </summary>
        Task UploadShipmentDetails(IEnumerable<long> shipmentKeys);
    }
}