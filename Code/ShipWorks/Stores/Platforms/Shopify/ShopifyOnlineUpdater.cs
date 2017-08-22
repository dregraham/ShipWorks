using System;
using System.Threading.Tasks;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using log4net;
using ShipWorks.Shipping;
using ShipWorks.Stores.Platforms.Shopify.Enums;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Updates Shopify order status/shipments
    /// </summary>
    public class ShopifyOnlineUpdater
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShopifyOnlineUpdater));
        private readonly ShopifyStoreEntity shopifyStore;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyOnlineUpdater(ShopifyStoreEntity store)
        {
            shopifyStore = store;
        }

        /// <summary>
        /// Push the online status for an order.
        /// </summary>
        public async Task UpdateOnlineStatus(ShopifyOrderEntity order)
        {
            if (order == null)
            {
                throw new ArgumentNullException("order", "order is required.");
            }

            // upload tracking number for the most recent processed, not voided shipment
            ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(order.OrderID);
            if (shipment == null)
            {
                // log that there was no shipment, and return
                log.DebugFormat("There was no shipment found for order Id: {0}", order.OrderID);
                return;
            }

            await UpdateOnlineStatus(shipment).ConfigureAwait(false);
        }

        /// <summary>
        /// Push the online status for an shipment.
        /// </summary>
        public async Task UpdateOnlineStatus(ShipmentEntity shipment)
        {
            UnitOfWork2 unitOfWork = new UnitOfWork2();
            await UpdateOnlineStatus(shipment, unitOfWork).ConfigureAwait(false);

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                unitOfWork.Commit(adapter);
                adapter.Commit();
            }
        }

        /// <summary>
        /// Push the online status for an shipment.
        /// </summary>
        public async Task UpdateOnlineStatus(long shipmentID, UnitOfWork2 unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("unitOfWork", "unitOfWork is required");
            }

            ShipmentEntity shipment = ShippingManager.GetShipment(shipmentID);
            if (shipment == null)
            {
                log.WarnFormat("Not updating status of shipment {0} as it has gone away.", shipmentID);
                return;
            }

            await UpdateOnlineStatus(shipment, unitOfWork).ConfigureAwait(false);
        }

        /// <summary>
        /// Push the online status for an shipment.
        /// </summary>
        public async Task UpdateOnlineStatus(ShipmentEntity shipment, UnitOfWork2 unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("unitOfWork", "unitOfWork is required");
            }

            if (shipment.Order.IsManual)
            {
                log.WarnFormat("Not updating order {0} since it is manual.", shipment.Order.OrderNumberComplete);
                return;
            }

            ShopifyOrderEntity order = (ShopifyOrderEntity) shipment.Order;

            ShopifyWebClient webClient = new ShopifyWebClient(shopifyStore, null);
            await webClient.UploadOrderShipmentDetails(shipment).ConfigureAwait(false);

            order.FulfillmentStatusCode = (int) ShopifyFulfillmentStatus.Fulfilled;
            order.OnlineStatus = EnumHelper.GetDescription(ShopifyStatus.Shipped);

            unitOfWork.AddForSave(order);
        }
    }
}
