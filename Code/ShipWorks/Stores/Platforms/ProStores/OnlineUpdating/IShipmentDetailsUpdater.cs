using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShipWorks.Stores.Platforms.ProStores.OnlineUpdating
{
    /// <summary>
    /// Utility class for updating the online status of ProStores orders
    /// </summary>
    public interface IShipmentDetailsUpdater
    {
        /// <summary>
        /// Upload the shipment details for the given order keys
        /// </summary>
        Task UploadOrderShipmentDetails(IEnumerable<long> orderKeys);

        /// <summary>
        /// Uploads shipment details for a particular shipment
        /// </summary>
        Task UploadShipmentDetails(IEnumerable<long> shipmentKeys);
    }
}