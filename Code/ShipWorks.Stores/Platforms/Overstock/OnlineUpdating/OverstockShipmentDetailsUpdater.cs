using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Overstock
{
    /// <summary>
    /// Handles uploading data to Overstock
    /// </summary>
    [Component]
    public class OverstockShipmentDetailsUpdater : IOverstockShipmentDetailsUpdater
    {
        /// <summary>
        /// Uploads shipment details for the given shipment Id
        /// </summary>
        public Task UploadShipmentDetails(long shipmentID)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Uploads shipment details for the given shipment entity
        /// </summary>
        public Task UploadShipmentDetails(ShipmentEntity shipment)
        {
            return Task.CompletedTask;
        }
    }
}