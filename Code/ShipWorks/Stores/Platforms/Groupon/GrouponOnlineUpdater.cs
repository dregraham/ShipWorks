using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using log4net;
using ShipWorks.Stores.Content;
using ShipWorks.Shipping;
using ShipWorks.Stores.Platforms.Groupon;
using ShipWorks.Stores.Platforms.Groupon.DTO;
using System.Threading;

namespace ShipWorks.Stores.Platforms.Groupon
{
    /// <summary>
    /// Uploads shipment details to Groupon
    /// </summary>
    public class GrouponOnlineUpdater
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(GrouponOnlineUpdater));

        // the store this instance for
        GrouponStoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public GrouponOnlineUpdater(GrouponStoreEntity store)
        {
            this.store = store;
        }

        /// <summary>
        /// Update the online status of the given order
        /// </summary>
        public void UpdateShipmentDetails(IEnumerable<long> orderKeys)
        {
            List<ShipmentEntity> shipments = new List<ShipmentEntity>();

            foreach (long orderID in orderKeys)
            {
                // upload tracking number for the most recent processed, not voided shipment
                ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(orderID);
                if (shipment == null)
                {
                    log.InfoFormat("There were no Processed and not Voided shipments to upload for OrderID {0}.", orderID);
                }
                else
                {
                    UpdateShipmentDetails(shipment);
                }
            }
        }

        /// <summary>
        /// Push the online status for an order.
        /// </summary>
        public void UpdateShipmentDetails(OrderEntity order)
        {
            // upload tracking number for the most recent processed, not voided shipment
            ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(order.OrderID);
            if (shipment == null)
            {
                // log that there was no shipment, and return
                log.DebugFormat("There was no shipment found for order Id: {0}", order.OrderID);
                return;
            }

            UpdateShipmentDetails(shipment);
        }

        /// <summary>
        /// Push the shipment details to the store.
        /// </summary>
        public void UpdateShipmentDetails(long shipmentID)
        {
            ShipmentEntity shipment = ShippingManager.GetShipment(shipmentID);
            if (shipment == null)
            {
                log.WarnFormat("Not updating status of shipment {0} as it has gone away.", shipmentID);
                return;
            }

            UpdateShipmentDetails(shipment);
        }

        /// <summary>
        /// Push the online status for an shipment.
        /// </summary>
        private void UpdateShipmentDetails(ShipmentEntity shipment)
        {
            OrderEntity order = shipment.Order;
            if (order.IsManual)
            {
                log.WarnFormat("Not updating order {0} since it is manual.", shipment.Order.OrderNumberComplete);
                return;
            }

            // Fetch the order items
            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.FetchEntityCollection(order.OrderItems, new RelationPredicateBucket(OrderItemFields.OrderID == order.OrderID));
            }

            // Groupon requires order items to create a shipment, so make sure we have some
            if (order.OrderItems == null || order.OrderItems.Count == 0)
            {
                throw new GrouponException(string.Format("Unable to upload shipment details because no order items were found for order number {0}", order.OrderNumber));
            }

            GrouponWebClient client = new GrouponWebClient(store);

            foreach(GrouponOrderItemEntity item in order.OrderItems)
            {
                client.UploadShipmentDetails(shipment, item.CILineItemID);
            }
        }
    }
}