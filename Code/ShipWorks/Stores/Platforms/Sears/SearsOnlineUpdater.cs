using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;

namespace ShipWorks.Stores.Platforms.Sears
{
    /// <summary>
    /// Online
    /// </summary>
    public class SearsOnlineUpdater
    {
        static readonly ILog log = LogManager.GetLogger(typeof(SearsOnlineUpdater));

        /// <summary>
        /// Updload the shipment details for the given shipment ID
        /// </summary>
        public void UploadShipmentDetails(long shipmentID)
        {
            ShipmentEntity shipment = ShippingManager.GetShipment(shipmentID);

            if (shipment == null)
            {
                log.InfoFormat("Not uploading tracking number for shipment {0}, shipment was deleted.", shipmentID);
                return;
            }

            UploadShipmentDetails(shipment);
        }

        /// <summary>
        /// Upload the shipment details for the given shipment
        /// </summary>
        public void UploadShipmentDetails(ShipmentEntity shipment)
        {
            SearsOrderEntity order = shipment.Order as SearsOrderEntity;
            if (order == null)
            {
                log.Error("shipment not associated with order in SearsOnlineUpdater");
                throw new SearsException("Shipment not associated with order");
            }

            if (order.IsManual)
            {
                log.InfoFormat("Not uploading shipment details for manual order {0}", order.OrderNumberComplete);
                return;
            }

            SearsStoreEntity storeEntity = StoreManager.GetRelatedStore(order.OrderID) as SearsStoreEntity;
            if (storeEntity == null)
            {
                log.InfoFormat("Could not find the sears store for order {0}", order.OrderNumberComplete);
                return;
            }

            SearsWebClient webClient = new SearsWebClient(storeEntity);
            webClient.UploadShipmentDetails(shipment);
        }
    }
}
