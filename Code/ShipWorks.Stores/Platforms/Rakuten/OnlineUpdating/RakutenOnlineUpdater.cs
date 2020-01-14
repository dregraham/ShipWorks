using System;
using System.Net;
using System.Threading.Tasks;
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
        private readonly IRakutenOrderSearchProvider orderSearchProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public RakutenOnlineUpdater(IRakutenWebClient updateClient,
            IRakutenOrderSearchProvider orderSearchProvider,
            Func<Type, ILog> createLogger)
        {
            this.updateClient = updateClient;
            this.orderSearchProvider = orderSearchProvider;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Upload the tracking number of the shipment
        /// </summary>
        public async Task UploadTrackingNumber(IRakutenStoreEntity store, long shipmentID)
        {
            ShipmentEntity shipment = ShippingManager.GetShipment(shipmentID);
            if (shipment == null)
            {
                log.InfoFormat("Not uploading tracking number for shipment {0}, order was deleted.", shipmentID);
            }
            else
            {
                await UploadTrackingNumber(store, shipment).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Upload the tracking number of the shipment
        /// </summary>
        public async Task UploadTrackingNumber(IRakutenStoreEntity store, ShipmentEntity shipment)
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

                try
                {
                    var identifiers = await orderSearchProvider.GetOrderIdentifiers(order).ConfigureAwait(false);

                    foreach (var identifier in identifiers)
                    {
                        // Upload tracking number
                        await updateClient.ConfirmShipping(store, shipment, identifier).ConfigureAwait(false);
                    }
                }
                catch (Exception ex) when (ex is ArgumentException || ex is WebException)
                {
                    log.Error($"An error occurred uploading tracking for order {order.OrderNumberComplete}: {ex.Message}");
                }
            }
            else
            {
                log.InfoFormat("Not uploading tracking number since order {0} is manual.", order.OrderNumberComplete);
            }
        }
    }
}
