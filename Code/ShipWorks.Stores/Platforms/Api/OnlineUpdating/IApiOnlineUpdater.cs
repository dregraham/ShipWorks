using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Api.OnlineUpdating
{
    /// <summary>
    /// Uploads shipment details to Api
    /// </summary>
    public interface IApiOnlineUpdater
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