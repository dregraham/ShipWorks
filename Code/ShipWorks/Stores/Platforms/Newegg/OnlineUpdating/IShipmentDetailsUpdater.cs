using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.Newegg.OnlineUpdating
{
    /// <summary>
    /// Update Newegg shipment details online
    /// </summary>
    public interface IShipmentDetailsUpdater
    {
        /// <summary>
        /// Uploads the shipping details to Newegg.
        /// </summary>
        Task UploadShippingDetails(INeweggStoreEntity store, ShipmentEntity shipmentEntity);
    }
}
