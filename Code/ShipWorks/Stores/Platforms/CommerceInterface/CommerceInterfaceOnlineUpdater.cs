using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;
using log4net;
using ShipWorks.Shipping;

namespace ShipWorks.Stores.Platforms.CommerceInterface
{
    /// <summary>
    /// Online updater for CommerceInterface
    /// </summary>
    public class CommerceInterfaceOnlineUpdater : GenericStoreOnlineUpdater
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(CommerceInterfaceOnlineUpdater));

        /// <summary>
        /// Constructor
        /// </summary>
        public CommerceInterfaceOnlineUpdater(GenericModuleStoreEntity store) : base(store)
        {
        }

        /// <summary>
        /// Posts the tracking number for the identified shipment to the store
        /// </summary>
        public async Task UploadTrackingNumber(long shipmentID, int statusCode)
        {
            ShipmentEntity shipment = ShippingManager.GetShipment(shipmentID);
            if (shipment == null)
            {
                log.InfoFormat("Not uploading tracking number for shipment {0}, shipment was deleted.", shipmentID);
                return;
            }

            await UploadTrackingNumber(shipment, statusCode).ConfigureAwait(false);
        }

        /// <summary>
        /// Posts the tracking number for the identified shipment to the store
        /// </summary>
        public async Task UploadTrackingNumber(ShipmentEntity shipment, int statusCode)
        {
            if (!shipment.Processed || shipment.Voided)
            {
                log.InfoFormat("Not uploading tracking number for shipment {0}, either not processed or has been voided.", shipment.ShipmentID);
                return;
            }

            OrderEntity order = shipment.Order;
            if (!order.IsManual)
            {
                // Upload tracking number
                CommerceInterfaceWebClient webClient = (CommerceInterfaceWebClient)GenericStoreType.CreateWebClient();
                await webClient.UploadShipmentDetails(order, shipment, statusCode).ConfigureAwait(false);
            }
            else
            {
                log.InfoFormat("Not uploading tracking number since order {0} is manual.", order.OrderID);
            }
        }
    }
}
