using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Api
{
    /// <summary>
    /// Interface for online updaters
    /// </summary>
    public interface IOnlineUpdater
    {
        /// <summary>
        /// Uploads shipment details for a particular shipment
        /// </summary>
        Task UploadShipmentDetails(StoreEntity store, List<ShipmentEntity> shipments);
    }
}
