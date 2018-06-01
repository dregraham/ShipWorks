using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Overstock
{
    /// <summary>
    /// Handles uploading data to Overstock
    /// </summary>
    public interface IOverstockShipmentDetailsUpdater
    {
        /// <summary>
        /// Uploads shipment details for the given shipment Id
        /// </summary>
        Task UploadShipmentDetails(long shipmentID);

        /// <summary>
        /// Uploads shipment details for the given shipment entity
        /// </summary>
        Task UploadShipmentDetails(ShipmentEntity shipment);
    }
}