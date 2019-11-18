using System;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;

namespace ShipWorks.Stores.Platforms.Rakuten.OnlineUpdating
{
    /// <summary>
    /// Posts shipping information
    /// </summary>
    [Component]
    public class RakutenOnlineUpdater : IRakutenOnlineUpdater
    {
        private readonly ILog log;
        private readonly IRakutenWebClient updateClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public RakutenOnlineUpdater(IRakutenWebClient updateClient,
            Func<Type, ILog> createLogger)
        {
            this.updateClient = updateClient;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Upload the tracking number of the shipment
        /// </summary>
        public void UploadTrackingNumber(IRakutenStoreEntity store, long shipmentID)
        {
            ShipmentEntity shipment = ShippingManager.GetShipment(shipmentID);
            if (shipment == null)
            {
                log.InfoFormat("Not uploading tracking number for shipment {0}, order was deleted.", shipmentID);
            }
            else
            {
                UploadTrackingNumber(store, shipment);
            }
        }

        /// <summary>
        /// Upload the tracking number of the shipment
        /// </summary>
        public void UploadTrackingNumber(IRakutenStoreEntity store, ShipmentEntity shipment)
        {
            if (!shipment.Processed || shipment.Voided)
            {
                log.InfoFormat("Not uploading tracking number since shipment ID {0} is not processed or is voided.", shipment.ShipmentID);
                return;
            }

            OrderEntity order = shipment.Order;

            if (!order.IsManual)
            {
                try
                {
                    ShippingManager.EnsureShipmentLoaded(shipment);
                }
                catch (ObjectDeletedException)
                {
                    // Shipment was deleted
                    return;
                }
                catch (SqlForeignKeyException)
                {
                    // Shipment was deleted
                    return;
                }

                // Upload tracking number
                updateClient.ConfirmShipping(store, shipment);
            }
            else
            {
                log.InfoFormat("Not uploading tracking number since order {0} is manual.", order.OrderID);
            }
        }
    }
}
