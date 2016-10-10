using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;

namespace ShipWorks.Stores.Platforms.OrderMotion
{
    /// <summary>
    /// Handles uploading data to OrderMotion
    /// </summary>
    public class OrderMotionOnlineUpdater
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(OrderMotionOnlineUpdater));

        // store for which this updater is to operate
        private readonly OrderMotionStoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderMotionOnlineUpdater(OrderMotionStoreEntity store)
        {
            this.store = store;
        }

        /// <summary>
        /// Uploads shipment details for the given shipment Id
        /// </summary>
        public void UploadShipmentDetails(long shipmentID)
        {
            ShipmentEntity shipment = ShippingManager.GetShipment(shipmentID);
            if (shipment == null)
            {
                log.InfoFormat("Not uploading tracking number for shipoment {0}, shipment was deleted.", shipmentID);
                return;
            }

            UploadShipmentDetails(shipment);
        }

        /// <summary>
        /// Uploads shipment details for the given shipment entity
        /// </summary>
        public void UploadShipmentDetails(ShipmentEntity shipment)
        {
            if (!shipment.Processed || shipment.Voided)
            {
                log.InfoFormat("Not uploading shipment details for shipment {0}, either not processed or has been voided.", shipment.ShipmentID);
                return;
            }

            OrderEntity order = shipment.Order;
            if (!order.IsManual)
            {
                // Upload tracking number
                OrderMotionWebClient client = new OrderMotionWebClient(store);

                // upload the details
                client.UploadShipmentDetails(shipment);
            }
            else
            {
                log.InfoFormat("Not uploading tracking number since order {0} is manual.", order.OrderID);
            }
        }
    }
}
